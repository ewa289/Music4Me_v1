using Music4Me.Common.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Http
{
    public abstract class ApiClientSettings : ISettings
    {
        public string EndPoint { get; set; }
    }
}
