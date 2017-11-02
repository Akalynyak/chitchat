using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discussions
{
    [Activity(Label = "Chit-Chat!", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        List<Discussion> prevDiscussions;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            prevDiscussions = new List<Discussion>();

            if (Intent.HasExtra("dis"))
            {
                Discussion discussion = JsonConvert.DeserializeObject<Discussion>(Intent.GetStringExtra("dis"));
                if (discussion != null)
                {
                    prevDiscussions.Add(discussion);
                }
            }

            var tab = this.ActionBar.NewTab();
            tab.SetText(GetString(Resource.String.tab1text));
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Replace(Resource.Id.fragmentContainer, new NewTabFragment());
            };
            ActionBar.AddTab(tab);

            tab = this.ActionBar.NewTab();
            tab.SetText(GetString(Resource.String.tab2text));
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                Bundle args = new Bundle();
                var listFragment = new ListTabFragment();
                listFragment.Arguments = args;
                args.PutString("prevdis", JsonConvert.SerializeObject(prevDiscussions));
                e.FragmentTransaction.Replace(Resource.Id.fragmentContainer, listFragment);
            };
            ActionBar.AddTab(tab);

        }
    }
}

