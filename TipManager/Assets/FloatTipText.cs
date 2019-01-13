using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FloatTipText : MonoBehaviour
{
    protected class Entry
    {
        public Transform root;
        public float time;
        public float stay = 0f;
        public float offset = 0f;
        public UILabel label;
        public UISprite sprite;
        public UISprite itemSprite;


        public float movementStart { get { return time + stay; } }
    }

    static int Comparison(Entry a, Entry b)
    {
        if (a.movementStart < b.movementStart) return -1;
        if (a.movementStart > b.movementStart) return 1;
        return 0;
    }

    public GameObject prefab;

    public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 80f) });

    public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f, 0f) });

    Queue<Entry> mList = new Queue<Entry>();
    Queue<Entry> mUnused = new Queue<Entry>();

    float counter = 0;
    public bool isVisible { get { return mList.Count != 0; } }

    Entry Create()
    {
        if (mUnused.Count > 0)
        {
            Entry ent = mUnused.Dequeue();
            ent.time = Time.realtimeSinceStartup;
            ent.offset = 0f;
            mList.Enqueue(ent);
            return ent;
        }

        Entry ne = new Entry();
        ne.time = Time.realtimeSinceStartup;
        ne.root = NGUITools.AddChild(gameObject, prefab).transform;
        ne.root.name = string.Format("_tip_{0}", counter.ToString());
        ne.label = ne.root.Find("Label").GetComponent<UILabel>();
        ne.sprite = ne.root.Find("Sprite").GetComponent<UISprite>();

        NGUITools.SetActive(ne.root.gameObject, false);
        mList.Enqueue(ne);
        ++counter;
        return ne;

    }

    void Delete(Entry ent)
    {
        mList.Dequeue();
        mUnused.Enqueue(ent);
        NGUITools.SetActive(ent.root.gameObject, false);
        ent.root.localPosition = new Vector3(0, mStartPosY, 0);
        ent.root.DOKill();
    }

    /*UIAtlas atlas, string icon,*/
    public void Add(string txt, Color c, float stayDuration)
    {
        if (!enabled) return;

        if (prefab == null) return;

        bool changeRaw = false;
        int count = 0;

        if (txt.Contains("\n"))
        {
            changeRaw = true;
        }
        else
        {
            for (int i = 0; i < txt.Length; i++)
            {
                string sTemp = txt.Substring(i, 1);
                byte[] byte_len = System.Text.Encoding.UTF8.GetBytes(sTemp);
                if (byte_len.Length > 1)
                {
                    count++;
                }
                if (count > 20)
                {
                    txt = txt.Insert(i, "\n");
                    count = 0;
                    changeRaw = true;
                }
            }
        }

        Entry ne = Create();
        ne.stay = stayDuration;

        if (ne.label != null)
        {
            ne.label.color = c;
            ne.label.text = txt;
            ne.label.alignment = changeRaw ? NGUIText.Alignment.Left : NGUIText.Alignment.Center;

        }
    }

    public float TotalEnd = 3.0f; //显示时长
    private float mStartPosY = 70;//起始位置
    private int mItemInterval = 10;


    public void OnDisable()
    {

    }

    public void Clean()
    {

    }

    void Update()
    {
        if (mList.Count == 0)
        {
            return;
        }

        float time = Time.realtimeSinceStartup;

        Entry[] list = mList.ToArray();
        for (int i = 0, len = list.Length; i < len; i++)
        {
            Entry ent = list[i];

            float currentTime = time - ent.movementStart;

            if (currentTime > TotalEnd)
            {
                Delete(ent);
                continue; ;
            }
            else
            {
                NGUITools.SetActive(ent.root.gameObject, true);
            }

            var tPos = mStartPosY;
            if (i < len - 1)
            {

                for (int j = len - 1; j > i; j--)
                {
                    if (j >= 0 && j < list.Length)
                        tPos += list[j].label.printedSize.y;
                }

                tPos += (len - 1 - i) * mItemInterval; //间隔
                ent.root.DOLocalMoveY(tPos, 0.2f).SetDelay(0.05f);
            }
            else
            {
                ent.root.localPosition = new Vector3(0f, tPos, -1 * i);
            }
        }

    }



}

