using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを操作するために必要

public class MicrophoneToSpeaker : MonoBehaviour
{
    private AudioSource audioSource;
    private string microphoneName;
    private bool isRecording = false;
    public Text buttonText; // ボタンのテキスト

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        // 使用可能なマイクデバイスを取得
        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0]; // 最初のマイクを使用
            Debug.Log($"使用中のマイク: {microphoneName}");
        }
        else
        {
            Debug.LogError("マイクが接続されていません。");
        }
    }

    // マイク入力を開始
    public void StartRecording()
    {
        if (microphoneName != null)
        {
            // マイク入力をAudioClipに設定
            audioSource.clip = Microphone.Start(microphoneName, true, 1, 44100);
            
            // マイクが準備できるまで待つ
            while (!(Microphone.GetPosition(microphoneName) > 0)) { }

            Debug.Log("マイク入力開始");
            
            // マイク音声をループ再生
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // マイク入力を停止
    public void StopRecording()
    {
        if (microphoneName != null)
        {
            Debug.Log("マイク入力停止");
            Microphone.End(microphoneName);
            audioSource.Stop();
        }
    }

    public void OnClick()
    {
        if (isRecording)
        {
            buttonText.text = "Start";
            isRecording = false;
            StopRecording();
        }
        else
        {
            buttonText.text = "Stop";
            isRecording = true;
            StartRecording();
        }
    }

    void OnDisable()
    {
        // マイクを停止
        if (microphoneName != null)
        {
            Microphone.End(microphoneName);
        }
    }
}
