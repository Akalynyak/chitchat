using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace Discussions
{
    class Discussion
    {
        public string Name;
        public TimeSpan SpeechTime;
        public List<Participant> Participants;

        public Discussion(string name, TimeSpan speechTime)
        {
            Name = name;
            SpeechTime = speechTime;
            Participants = new List<Participant>();
        }
    }
}