using Ds3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ds3.Helpers.Strategies.ChunkStrategies
{
    public class WriteAggregateJobsChunkStrategy : IChunkStrategy
    {
        private IChunkStrategy _writeRandomAccessChunkStrategy;
        private IEnumerable<string> _objects;
        private MasterObjectList _jobResponse;
        
        public WriteAggregateJobsChunkStrategy(IEnumerable<Ds3Object> objects, int retryAfter = -1)
            : this(Thread.Sleep, objects, retryAfter)
        {
        }

        public WriteAggregateJobsChunkStrategy(Action<TimeSpan> wait, IEnumerable<Ds3Object> objects, int retryAfter = -1)
        {
            _writeRandomAccessChunkStrategy = new WriteRandomAccessChunkStrategy(wait, retryAfter);
            this._objects = objects.Select(obj => obj.Name);
        }

        public IEnumerable<TransferItem> GetNextTransferItems(IDs3Client client, MasterObjectList jobResponse)
        {
            this._jobResponse = jobResponse;
            _jobResponse.Objects = FilterObjectsForCurrentJob();
            return _writeRandomAccessChunkStrategy.GetNextTransferItems(client, _jobResponse);
        }

        public void CompleteBlob(Blob blob)
        {
            _writeRandomAccessChunkStrategy.CompleteBlob(blob);
        }

        public void Stop()
        {
            _writeRandomAccessChunkStrategy.Stop();
        }

        /// <summary>
        /// Filtering out objects that does not belong to the current running job
        /// </summary>
        /// <returns> A new list of objects to be processed by the running job</returns>
        private IEnumerable<Objects> FilterObjectsForCurrentJob()
        {

            var filteredObjectsList = new List<Objects>();

            foreach (var objectList in _jobResponse.Objects)
            {
                IList<BulkObject> filter = new List<BulkObject>();
                foreach(var obj in objectList.ObjectsList)
                {
                    if (_objects.Contains(obj.Name))
                    {
                        filter.Add(obj);
                    }
                }

                if (filter.Any())
                {
                    var objects = new Objects
                    {
                        ChunkId = objectList.ChunkId,
                        ChunkNumber = objectList.ChunkNumber,
                        NodeId = objectList.NodeId,
                        ObjectsList = filter
                    };
                    filteredObjectsList.Add(objects);
                }
            }

            return filteredObjectsList;
        }
    }
}
