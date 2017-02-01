using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;
using SpeechRecognitionDemo.SpeechRecognition;

namespace SpeechRecognitionDemo
{
    [Activity(Label = "SpeechRecognitionDemo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISpeechRecognitionListener
    {
        private TextView _textView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _textView = FindViewById<TextView>(Resource.Id.TextView);

            var intent = new Intent(this, typeof(SpeechRecognitionService));
            BindService(intent, new SpeechRecognitionServiceConnection(this), Bind.AutoCreate);
        }

        public void OnSpeechRecognitionError(SpeechRecognizerError error)
        {
            Toast.MakeText(this, error.ToString(), ToastLength.Short).Show();
        }

        public void OnSpeechRecognitionResults(IEnumerable<string> results)
        {
            _textView.Text = results.First();
        }
    }
}

