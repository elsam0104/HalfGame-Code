using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using PROTO;
using UnityEngine.Assertions;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject otherArea, mineArea, noticeArea;

    protected Area LastArea;
    protected bool isother = false;
    private AsyncOperationHandle handle;
    public void IsOtherPeople(Toggle isOther)
    {
        isother = isOther.isOn;
    }
    protected void ChatClear(RectTransform contentRect)
    {
        foreach (Transform child in contentRect.transform)
        {
            Destroy(child.gameObject);
        }
    }

    protected void Notice(RectTransform contentRect, Scrollbar scrollbar, string notice)
    {
        Area area = CreatArea(contentRect, noticeArea, notice);

        Fit(area.BoxRect);
        Fit(area.AreaRect);
        Fit(contentRect);

        LastArea = area;

        StartCoroutine(ScrollDelay(scrollbar));
    }
    protected void ImageNotice(RectTransform contentRect, Scrollbar scrollbar, string texture, string text, GameObject prefab)
    {
        Area area = CreatArea(contentRect, prefab, text);

        Addressables.LoadAssetAsync<Sprite>(texture).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                handle = obj;
                Image image = area.DescriptionImage;
                image.sprite = obj.Result;
                Fit(area.BoxRect);
                Fit(area.AreaRect);
                Fit(contentRect);

                LastArea = area;
                StartCoroutine(ScrollDelay(scrollbar));
            };
    }
    protected void Chat(RectTransform contentRect, Scrollbar scrollbar, bool isSend, string text, string user, Sprite picture = null, ChatType chatType = ChatType.Dayalight)
    {
        if (text.Trim() == "") return; //�����̽�, ���� �ɷ���
        text = text.Trim();
        bool isBottom = scrollbar.value <= 0.1f;

        Area area = CreatArea(contentRect, isSend ? mineArea : otherArea, text);

        if (picture != null && area.UserImage != null)
            area.UserImage.sprite = picture;
        switch (chatType)
        {
            case ChatType.Dead:
                {
                    area.BoxRect.gameObject.GetComponent<Image>().color = Color.HSVToRGB(0, 0, 0.55f);
                    area.Trail.gameObject.GetComponent<Image>().color = Color.HSVToRGB(0, 0, 0.55f);
                    break;
                }
            case ChatType.Night:
                {
                    area.BoxRect.gameObject.GetComponent<Image>().color = new Color32(151, 191, 209, 255);
                    area.Trail.gameObject.GetComponent<Image>().color = new Color32(151, 191, 209, 255);
                    break;
                }
            case ChatType.Dayalight: // default
                break;
            case ChatType.Listen:
                {
                    //��û
                    area.BoxRect.gameObject.GetComponent<Image>().color = new Color32(134, 255, 166, 255);
                    area.Trail.gameObject.GetComponent<Image>().color = new Color32(151, 191, 209, 255);
                    var tmp = area.TextRect.GetComponent<TMP_Text>();
                    tmp.text = $"..{text}...";
                    tmp.fontStyle = FontStyles.Italic;
                    break;
                }
            default:
                break;
        }

        Fit(area.BoxRect);
        FitTextHeight(area);

        SetTime(area, user, isSend);

        Fit(area.BoxRect);
        Fit(area.AreaRect);
        Fit(contentRect);
        LastArea = area;

        if (isSend || isBottom)
            StartCoroutine(ScrollDelay(scrollbar));
    }
    protected void Fit(RectTransform rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    private IEnumerator ScrollDelay(Scrollbar scrollbar)
    {
        yield return new WaitForSeconds(0.03f);
        scrollbar.value = 0;
    }
    protected Area CreatArea(RectTransform contentRect, GameObject obj, string text)
    {
        Area area = Instantiate(obj).GetComponent<Area>();
        area.transform.SetParent(contentRect.transform, false);
        area.BoxRect.sizeDelta = new Vector2(600, area.BoxRect.sizeDelta.y);
        area.TextRect.GetComponent<TMP_Text>().text = text;

        return area;
    }

    private void SetTime(Area area, string user, bool isSend)
    {
        //���� �Ϳ� �б��� ������ ��¥�� ���� �̸� ����
        DateTime t = DateTime.Now;
        area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        area.User = user;

        //�ð��� ������ �������� area�� ���ο� �ð� ����
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        area.TimeText.text = (t.Hour > 12 ? "���� " : "���� ") + hour + ":" + t.Minute.ToString("D2");

        //���� �ð��� ���� �ö��� ���´��� �Ǻ�
        bool isSameUser = LastArea != null && LastArea.Time == area.Time && LastArea.User == area.User;
        bool isSameTime = LastArea != null && LastArea.Time == area.Time;
        //���� ���� �ð��� �������� ���� �ð�, ���� ���ֱ�
        if (isSameUser)
            LastArea.TimeText.text = "";
        area.Trail.SetActive(!isSameUser);
        //Ÿ���� ���� �ð��� �������� ����, �̸� ���ֱ�
        if (!isSend)
        {
            area.UserImage.gameObject.SetActive(!isSameUser);
            area.UserText.gameObject.SetActive(!isSameUser);
            area.UserText.text = area.User;
        }

        //���� �Ͱ� ��¥�� �ٸ��� ��¥ǥ���ϱ�
        //if (LastArea != null && LastArea.Time !="" && LastArea.Time.Substring(0, 10) != area.Time.Substring(0, 10))
        //{
        //    ShowDateArea(t);
        //}
    }
    protected void ShowDateArea(RectTransform contentRect, DateTime t)
    {
        Transform CurDateArea = Instantiate(noticeArea).transform;
        CurDateArea.SetParent(contentRect.transform, false);
        CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);
        string week = "";
        switch (t.DayOfWeek)
        {
            case DayOfWeek.Monday:
                week = "��";
                break;
            case DayOfWeek.Friday:
                week = "��";
                break;
            case DayOfWeek.Saturday:
                week = "��";
                break;
            case DayOfWeek.Sunday:
                week = "��";
                break;
            case DayOfWeek.Thursday:
                week = "��";
                break;
            case DayOfWeek.Tuesday:
                week = "ȭ";
                break;
            case DayOfWeek.Wednesday:
                week = "��";
                break;
            default:
                break;
        }
        CurDateArea.GetComponent<Area>().DateText.text = t.Year + "�� " + t.Month + "�� " + t.Day + "�� " + week + "����";
    }



    /// <summary>
    ///�� �� �̻��̸� ũ�⸦ �ٿ����鼭 �� ���� �Ʒ��� �������� �ٷ� �� ũ�⸦ ������ 
    /// </summary>
    private void FitTextHeight(Area area)
    {

        float X = area.TextRect.sizeDelta.x + 42;
        float Y = area.TextRect.sizeDelta.y;
        if (Y > 60) //�� �Ʒ� ������ ���� ���� �ʰ� == �� �� �̻�
        {
            String[] tmpStr = area.TextRect.GetComponent<TMP_Text>().text.Split('\n');
            bool isLong = false;
            for (int i = 0; i < tmpStr.Length; i++)
            {
                if (tmpStr[i].Length > 1)
                {
                    isLong = true;
                    break;
                }
            }
            if (!isLong)
            {
                area.BoxRect.sizeDelta = new Vector2(area.TextRect.sizeDelta.x + 42, area.TextRect.sizeDelta.y);
                return;
            }
            for (int i = 0; i < 200; i++)
            {
                area.BoxRect.sizeDelta = new Vector2(X - i * 2, area.BoxRect.sizeDelta.y);

                Fit(area.BoxRect);
                if (Y != area.TextRect.sizeDelta.y)
                {
                    area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y);
                    break;
                }
            }
        }
        else
        {
            area.BoxRect.sizeDelta = new Vector2(X, Y);
        }

    }
}
