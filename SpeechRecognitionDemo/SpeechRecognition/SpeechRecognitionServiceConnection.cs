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
using Debug = System.Diagnostics.Debug;

namespace SpeechRecognitionDemo.SpeechRecognition
{
    public class SpeechRecognitionServiceConnection : Java.Lang.Object, IServiceConnection
    {
        private readonly ISpeechRecognitionListener _speechRecognitionListener;

        public SpeechRecognitionServiceConnection(ISpeechRecognitionListener speechRecognitionListener)
        {
            _speechRecognitionListener = speechRecognitionListener;
        }

        public void OnServiceConnected(ComponentName name, IBinder binder)
        {
            var speechRecognitionBinder = binder as SpeechRecognitionBinder;
            Debug.Assert(speechRecognitionBinder != null, "SpeechRecognitionServiceConnection used with wrong service");

            speechRecognitionBinder.SpeechRecognitionListener = _speechRecognitionListener;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            throw new NotImplementedException();
        }
    }
}