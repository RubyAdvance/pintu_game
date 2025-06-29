using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image bg_Img;
    public Image main_pic_Img;
    public Image fingers_Img;
    public Sprite[] finger_Sprite;
    public RectTransform[] main_sub_pic_pos;
    public Image[] subImages;

    public HandMove handMove;

    private Vector2[] subpicOriginalPos = new Vector2[9];

    private Sprite[] allBackgrounds;

    private Sprite[] allMainPics;

    private Sprite[] allSubPics;
    /// <summary>
    /// key:level value:subpicArr
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="Sprite[]"></typeparam>
    /// <returns></returns>
    Dictionary<int, Sprite[]> spriteDic = new Dictionary<int, Sprite[]>();
    /// <summary>
    /// 当前抓住的subpic
    /// </summary>
    public SubPic currentSubPic;

    private static GameController _instance;

    public static GameController Instance
    {
       get { return _instance; }
    }

    private void Awake()
    {
        currentSubPic = null;
        LoadRes();
        InitLevel();
        handMove.Init(false);
        _instance = this;
    }

    private void LoadRes()
    {
        //加载所有的背景图
        allBackgrounds = Resources.LoadAll<Sprite>("Bg");
        //储存subpic的初始位置
        for (int i = 0; i < subImages.Length; i++)
        {
            subpicOriginalPos[i] = subImages[i].transform.localPosition;
        }
        //加载所有的mainPic
        allMainPics = Resources.LoadAll<Sprite>("pintuRes/MainPic");
        //加载所有的subpic
        allSubPics = Resources.LoadAll<Sprite>("pintuRes/SubPic");
        //按字典存储
        for (int i = 0; i < allMainPics.Length; i++)
        {
            if (!spriteDic.ContainsKey(i)) spriteDic[i] = new Sprite[9];
            var tempArr = new Sprite[9];
            for (int j = 0; j < 9; j++)
            {
                int index = i * 9 + j; // 计算当前图片在allMainPics中的索引
                if (index < allSubPics.Length)
                {
                    tempArr[j] = allSubPics[index];
                }
            }

            spriteDic[i] = tempArr; // 将分组后的数组存入字典
        }

    }

    void InitLevel()
    {
        bg_Img.sprite = allBackgrounds[Random.Range(0, allBackgrounds.Length)];
        //随机关卡
        var randomLevel = Random.Range(0, spriteDic.Count);
        var subpicArr = spriteDic[randomLevel];
        main_pic_Img.sprite = allMainPics[randomLevel];
        Shuffle(subpicArr); // 打乱数组
        //设置subpic
        for (int i = 0; i < subpicArr.Length; i++)
        {
            var targetIndex = subpicArr[i].name.Split('_')[1]; // 获取subpic的下标
            subImages[i].sprite = subpicArr[i];
            subImages[i].transform.localPosition = subpicOriginalPos[i];
            subImages[i].gameObject.name =targetIndex ; // 设置name
            subImages[i].transform.GetComponent<SubPic>().Init(targetIndex);
        }
        //设置手指
        fingers_Img.sprite = finger_Sprite[0];


    }


    /// <summary>
    /// Fisher-Yates洗牌算法打乱数组
    /// </summary>
    /// <typeparam name="T">数组元素类型</typeparam>
    /// <param name="array">要打乱的数组</param>
    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            // 随机选择一个0到i之间的索引
            int j = Random.Range(0, i + 1);

            // 交换元素
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

}
