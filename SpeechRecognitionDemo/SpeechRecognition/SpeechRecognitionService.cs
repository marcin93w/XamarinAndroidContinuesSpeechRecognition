using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SpeechRecognitionDemo.SpeechRecognition
{
    [Service]
    public class SpeechRecognitionService : Service
    {
        private SpeechRecognizer _speechRecognizer;
        private Intent _speechRecognizerIntent;
        private SpeechRecognitionBinder _binder;
        private bool _isWaitingForStartListening;

        public override void OnCreate()
        {
            base.OnCreate();
            InitializeSpeechRecognizer();
        }

        private void InitializeSpeechRecognizer()
        {
            _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            _speechRecognizer.Error += SpeechRecognizerOnError;
            _speechRecognizer.Results += SpeechRecognizerOnResults;
            _speechRecognizer.ReadyForSpeech += SpeechRecognizerOnReadyForSpeech;
            _speechRecognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            _speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel,
                RecognizerIntent.LanguageModelFreeForm);
            _speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraCallingPackage,
                this.PackageName);
        }

        public override IBinder OnBind(Intent intent)
        {
            _binder = new SpeechRecognitionBinder();
            StartListening();
            return _binder;
        }

        private void SpeechRecognizerOnResults(object sender, ResultsEventArgs resultsEventArgs)
        {
            var matches = resultsEventArgs.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            _binder.SpeechRecognitionListener?.OnSpeechRecognitionResults(matches);

            Log.Info("MyApp", $">>> {DateTime.Now.TimeOfDay} >>> Speech results: {matches.Count}");

            StartListening();
        }

        private void SpeechRecognizerOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            var error = errorEventArgs.Error;
            if (error != SpeechRecognizerError.SpeechTimeout)
            {
                _binder.SpeechRecognitionListener?.OnSpeechRecognitionError(error);
            }

            Log.Info("MyApp", $">>> {DateTime.Now.TimeOfDay} >>> Speech error: {error}");

            StartListening();
        }

        private void SpeechRecognizerOnReadyForSpeech(object sender, ReadyForSpeechEventArgs readyForSpeechEventArgs)
        {
            _isWaitingForStartListening = false;
        }

        public void StartListening()
        {
            StartListeningAfter1Sec();
        }

        private void StartListeningAfter1Sec()
        {
            if (!_isWaitingForStartListening)
            {
                _isWaitingForStartListening = true;
                Thread.Sleep(1000);
                _speechRecognizer.StartListening(_speechRecognizerIntent);
            }
        }
    }
}