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

namespace Discussions
{
    class Participant
    {
        public string Name;
        int Upvotes;
        int Downvotes;
        int Rating;
        int Mistakes;

        public Participant(string name)
        {
            Name = name;
            Upvotes = 0;
            Downvotes = 0;
            Rating = 0;
            Mistakes = 0;
        }

        public void Copy(Participant p)
        {
            Name = p.Name;
            Upvotes = p.GetUpvotes();
            Downvotes = p.GetDownvotes();
            Rating = p.GetRating();
            Mistakes = p.GetMistakes();
        }

        public void Upvote()
        {
            Upvotes++;
            Rating = Upvotes - Downvotes;
        }

        public void Downvote()
        {
            Downvotes++;
            Rating = Upvotes - Downvotes;
        }

        public void AddMistake()
        {
            Mistakes++;
        }

        public int GetUpvotes()
        {
            return Upvotes;
        }

        public int GetDownvotes()
        {
            return Downvotes;
        }

        public int GetRating()
        {
            return Rating;
        }

        public int GetMistakes()
        {
            return Mistakes;
        }

    }
}