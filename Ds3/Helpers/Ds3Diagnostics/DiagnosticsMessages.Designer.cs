﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ds3.Helpers.Ds3Diagnostics {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DiagnosticsMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DiagnosticsMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ds3.Helpers.Ds3Diagnostics.DiagnosticsMessages", typeof(DiagnosticsMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Found {0} cache file systems that near the capacity limit.
        /// </summary>
        internal static string FoundCacheNearCapacityLimit {
            get {
                return ResourceManager.GetString("FoundCacheNearCapacityLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Found {0} offline tapes.
        /// </summary>
        internal static string FoundOfflineTapes {
            get {
                return ResourceManager.GetString("FoundOfflineTapes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Found {0} powered off pools.
        /// </summary>
        internal static string FoundPowerOffPools {
            get {
                return ResourceManager.GetString("FoundPowerOffPools", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No cache file systems were found.
        /// </summary>
        internal static string NoCacheFound {
            get {
                return ResourceManager.GetString("NoCacheFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No tapes found in the system.
        /// </summary>
        internal static string NoTapesFound {
            get {
                return ResourceManager.GetString("NoTapesFound", resourceCulture);
            }
        }
    }
}
