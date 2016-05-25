using System;
using System.Collections.Generic;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Services 
{
    public sealed class Singleton
    {
        private static volatile Singleton _instance;
        private static readonly object SyncRoot = new Object();

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        _instance = new Singleton();
                    }
                }

                return _instance;
            }
        }

        public List<HouseEntity> Houses { get; set; }
    }
}