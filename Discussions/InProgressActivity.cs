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
using System.Threading;
using System.Threading.Tasks;

namespace Discussions
{
    [Activity(Label = "InProgressActivity", Icon = "@drawable/icon")]
    public class InProgressActivity : Activity
    {
        Discussion discussion;
        public int currentSpeakerId;
        Participant currentSpeaker;
        public bool over;
        public int countdownSeconds;
        public int countdownMinutes;
        TextView timerView;
        ImageButton startStopButton;
        System.Timers.Timer _timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InProgress);

            discussion = JsonConvert.DeserializeObject<Discussion>(Intent.GetStringExtra("dis"));

            this.Title = discussion.Name;

            over = false;
            currentSpeakerId = 0;
            currentSpeaker = discussion.Participants.ElementAt(currentSpeakerId);

            var currentSpeakerView = FindViewById<TextView>(Resource.Id.tvIPCurrentTopic);
            currentSpeakerView.Text = currentSpeaker.Name;
            timerView = FindViewById<TextView>(Resource.Id.tvIPTimer);

            var likesBar = FindViewById<ProgressBar>(Resource.Id.progressBarLikes);
            var dislikesBar = FindViewById<ProgressBar>(Resource.Id.progressBarDislikes);
            var likeButton = FindViewById<ImageButton>(Resource.Id.IPRightButton);
            var dislikeButton = FindViewById<ImageButton>(Resource.Id.IPLeftButton);
            var likeNumber = FindViewById<TextView>(Resource.Id.tvIPLikesNum);
            var dislikeNumber = FindViewById<TextView>(Resource.Id.tvIPDislikesNum);
            startStopButton = FindViewById<ImageButton>(Resource.Id.IPStartStopButton);
            var errorButton = FindViewById<Button>(Resource.Id.IPErrorButton);
            var nextSpeakerButton = FindViewById<Button>(Resource.Id.bIPNextSpeaker);

            likesBar.Max = discussion.Participants.Count;
            dislikesBar.Max = discussion.Participants.Count;

            _timer = new System.Timers.Timer(1000);
            _timer.Enabled = false;
            countdownMinutes = discussion.SpeechTime.Minutes;
            countdownSeconds = discussion.SpeechTime.Seconds;
            _timer.Elapsed += OnTimeEvent;

            timerView.Text = countdownMinutes.ToString() + ":" + countdownSeconds.ToString();

            likeButton.Click += (sender, e) =>
            {
                currentSpeaker.Upvote();
                RunOnUiThread(() =>
                {
                    likeNumber.Text = currentSpeaker.GetUpvotes().ToString();
                    likesBar.Progress += 1;
                });
                if ((currentSpeaker.GetUpvotes() + currentSpeaker.GetDownvotes()) == discussion.Participants.Count)
                {
                    likeButton.Enabled = false;
                    dislikeButton.Enabled = false;
                }
            };

            dislikeButton.Click += (sender, e) =>
            {
                currentSpeaker.Downvote();
                RunOnUiThread(() =>
                {
                    dislikeNumber.Text = currentSpeaker.GetDownvotes().ToString();
                    dislikesBar.Progress += 1;
                });
                if ((currentSpeaker.GetUpvotes() + currentSpeaker.GetDownvotes()) == discussion.Participants.Count)
                {
                    likeButton.Enabled = false;
                    dislikeButton.Enabled = false;
                }
            };

            errorButton.Click += (sender, e) =>
            {
                currentSpeaker.AddMistake();
            };

            startStopButton.Click += (sender, e) =>
            {
                if (!_timer.Enabled)
                {
                    startStopButton.SetImageResource(Resource.Drawable.ic_pause_white_36dp);
                    _timer.Enabled = true;
                    likeButton.Enabled = true;
                    dislikeButton.Enabled = true;
                    errorButton.Enabled = true;
                    nextSpeakerButton.Enabled = false;
                }
                else
                {
                    startStopButton.SetImageResource(Resource.Drawable.ic_play_arrow_white_36dp);
                    _timer.Enabled = false;
                    likeButton.Enabled = false;
                    dislikeButton.Enabled = false;
                    errorButton.Enabled = false;
                    nextSpeakerButton.Enabled = true;
                }
            };

            nextSpeakerButton.Click += (sender, e) =>
            {
                discussion.Participants.ElementAt(currentSpeakerId).Copy(currentSpeaker);
                currentSpeakerId++;
                if (currentSpeakerId < discussion.Participants.Count)
                {
                    currentSpeaker = discussion.Participants.ElementAt(currentSpeakerId);
                    countdownMinutes = discussion.SpeechTime.Minutes;
                    countdownSeconds = discussion.SpeechTime.Seconds;
                    RunOnUiThread(() =>
                    {
                        likeNumber.Text = currentSpeaker.GetUpvotes().ToString();
                        dislikeNumber.Text = currentSpeaker.GetDownvotes().ToString();
                        likesBar.Progress = 0;
                        dislikesBar.Progress = 0;
                        currentSpeakerView.Text = currentSpeaker.Name;
                        timerView.Text = countdownMinutes.ToString() + ":" + countdownSeconds.ToString();
                    });
                    startStopButton.Enabled = true;
                }
                else
                {
                    AlertDialog.Builder discussionEndAlert = new AlertDialog.Builder(this);
                    discussionEndAlert.SetTitle(Resource.String.IPEndAlertT);
                    discussionEndAlert.SetMessage(Resource.String.IPEndAlertM);
                    discussionEndAlert.SetNeutralButton(Resource.String.OK, (senderAlert, args) => 
                    {
                        var intent = new Intent(this, typeof(DiscussionResultActivity));
                        intent.PutExtra("dis", JsonConvert.SerializeObject(discussion));
                        StartActivity(intent);
                    });
                    discussionEndAlert.Show();
                }
            };

            likeButton.Enabled = false;
            dislikeButton.Enabled = false;
            errorButton.Enabled = false;
        }

        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (countdownSeconds > 0 || countdownMinutes > 0)
            {
                if (countdownSeconds == 0)
                {
                    countdownMinutes--;
                    countdownSeconds = 60;
                }
                else
                {
                    countdownSeconds--;
                }
                RunOnUiThread(() =>
                {
                    timerView.Text = countdownMinutes.ToString() + ":" + countdownSeconds.ToString();
                });
            }
            else
            {
                over = true;
                startStopButton.Enabled = false;
            }
        }
    }
}