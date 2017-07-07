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
        private IEnumerable<Ds3Object> _objects;
        private MasterObjectList _jobResponse;
        
        public WriteAggregateJobsChunkStrategy(IEnumerable<Ds3Object> objects, int retryAfter)
            : this(Thread.Sleep, objects, retryAfter)
        {
        }

        public WriteAggregateJobsChunkStrategy(Action<TimeSpan> wait, IEnumerable<Ds3Object> objects, int retryAfter)
        {
            _writeRandomAccessChunkStrategy = new WriteRandomAccessChunkStrategy(retryAfter);
            this._objects = objects;
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
                var filter = objectList.ObjectsList.Where(obj => _objects.Contains(new Ds3Object(obj.Name, obj.Length)));
                if (filter.Any())
                {
                    filteredObjectsList.Add(objectList);
                }
            }

            return filteredObjectsList;
        }
    }
}
