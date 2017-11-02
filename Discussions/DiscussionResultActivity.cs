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
using Newtonsoft.Json;

namespace Discussions
{
    [Activity(Label = "DiscussionResultActivity", Icon = "@drawable/icon")]
    public class DiscussionResultActivity : Activity
    {
        Discussion discussion;
        ListView list;
        ResultParticipantAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DiscussionResult);

            discussion = JsonConvert.DeserializeObject<Discussion>(Intent.GetStringExtra("dis"));

            string title = (GetString(Resource.String.DRFor) + " " + discussion.Name);
            this.Title = title;

            list = FindViewById<ListView>(Resource.Id.lvDRSpeakers);

            adapter = new ResultParticipantAdapter(this, discussion.Participants.ToArray());

            list.Adapter = adapter;

            var acceptButton = FindViewById<Button>(Resource.Id.bDROK);
            acceptButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("dis", JsonConvert.SerializeObject(discussion));
                intent.SetFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
            };

            // Create your application here
        }
    }
}