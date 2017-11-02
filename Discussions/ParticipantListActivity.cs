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
using Android.Views.InputMethods;
using Newtonsoft.Json;

namespace Discussions
{
    [Activity(Label = "ParticipantListActivity", Icon = "@drawable/icon")]
    public class ParticipantListActivity : Activity
    {
        Discussion discussion;
        List<Participant> items;
        ListView list;
        ParticipantAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ParticipantList);

            discussion = JsonConvert.DeserializeObject<Discussion>(Intent.GetStringExtra("dis"));

            string title = (GetString(Resource.String.PLFor) + " " + discussion.Name);
            this.Title = title;

            list = FindViewById<ListView>(Resource.Id.lvParticipantList);
            items = new List<Participant>();
            
            adapter = new ParticipantAdapter(this, items.ToArray());

            list.Adapter = adapter;

            var addParticipantButton = FindViewById<Button>(Resource.Id.bAddParticipant);
            addParticipantButton.Click += (sender, e) =>
            {
                Participant p = new Participant(string.Empty);
                EditParticipant(p);
                items.Add(p);
                RunOnUiThread(() => adapter.Update(items.ToArray()));
            };

            var confirmCreationButton = FindViewById<Button>(Resource.Id.bapConfirm);
            confirmCreationButton.Click += (sender, e) =>
            {
                if (items.Count != 0)
                {
                    discussion.Participants = items;
                    var intent = new Intent(this, typeof(InProgressActivity));
                    intent.PutExtra("dis", JsonConvert.SerializeObject(discussion));
                    StartActivity(intent);
                }
                else
                {
                    AlertDialog.Builder emptyListAlert = new AlertDialog.Builder(this);
                    emptyListAlert.SetTitle(Resource.String.PLEmptyAlertT);
                    emptyListAlert.SetMessage(Resource.String.PLEmptyAlertM);
                    emptyListAlert.SetNeutralButton(Resource.String.OK, (senderAlert, args) => { });
                    emptyListAlert.Show();
                }
            };
            // Create your application here
            RegisterForContextMenu(list);
            
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.lvParticipantList)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                menu.SetHeaderTitle(items[info.Position].Name);
                var menuItems = Resources.GetStringArray(Resource.Array.lvpContextMenu);
                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            if(item.ItemId == 0)
            {
                EditParticipant(items.ElementAt(info.Position));
            }
            else if(item.ItemId == 1)
            {
                items.Remove(items.ElementAt(info.Position));
            }
            RunOnUiThread(() => adapter.Update(items.ToArray()));
            return true;
        }

        void EditParticipant(Participant p)
        {
            var InputDialog = new AlertDialog.Builder(this);
            EditText etParticipantName = new EditText(this);
            if (p.Name != string.Empty)
            {
                etParticipantName.Text = p.Name;
            }
            InputDialog.SetTitle(Resource.String.NPDName);
            InputDialog.SetView(etParticipantName);
            InputDialog.SetPositiveButton(Resource.String.OK, (see, ess) =>
            {
                if (etParticipantName.Text != string.Empty)
                {
                    p.Name = etParticipantName.Text;
                }
                else
                {
                    p.Name = GetString(Resource.String.Unnamed);
                }
                RunOnUiThread(() => adapter.Update(items.ToArray()));
            });
            InputDialog.SetNegativeButton(Resource.String.Cancel, (afk, kfa) => { HideKeyboard(etParticipantName); });
            InputDialog.Show();
            ShowKeyboad(etParticipantName);
        }

        void ShowKeyboad(EditText userInput)
        {
            userInput.RequestFocus();
            InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            imm.ToggleSoftInput(ShowFlags.Forced, 0);
        }

        void HideKeyboard(EditText userInput)
        {
            InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(userInput.WindowToken, 0);
        }
    }
}