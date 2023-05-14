using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class DropManager : FileParser
{
    public List<bool> AutoSnapValues = new List<bool>();// { true, true, false, false, false, false, true, false, false, true };

    public List<DropZoneBase> DropZones = new List<DropZoneBase>();
    public List<DropZoneBase> DropZonesScenarioOne = new List<DropZoneBase>();
    public List<DropZoneBase> DropZonesScenarioTwo = new List<DropZoneBase>();
    public List<DraggedObject> draggedObjects;

    public AutoSnapping AutoSnappingScript;

    public int countOfActions = 0;
    public bool listIsOver;
    public bool additional;

    public VRTK_ControllerEvents LeftController;
    public VRTK_ControllerEvents RightController;

    public List<GameObject> objectsToDescription = new List<GameObject>();
    public List<GameObject> PortableObjectsList = new List<GameObject>();

    [SerializeField]
    protected Text DescriptionText;

    [SerializeField]
    private GetJson JsonParser;

    [SerializeField]
    private AudioSource audioSourceHelpPanel;
    [SerializeField]
    private AudioSource audioSourceCheckPanel;
    [HideInInspector]
    public string Scenario;

    [SerializeField]
    private GameObject scobeImageForTraining;

    protected int maxCountBeforeAutoSnap = 1;
    protected override void Awake()
    {
        base.Awake();

        Scenario = PlayerPrefs.GetString("Scenario");
        //if (scenario == "scenario2.txt")
        //{
        //    DropZones = DropZonesScenarioTwo;
        //    ///
        //    //УДАЛЕНИЕ СКРИПТОВ ПОВТОРЕНИЯ С РУБИЛЬНИКОВ
        //    ///
        //}
        //else
        //    DropZones = DropZonesScenarioOne;

        LeftController.GetComponent<VRTK_Pointer>().interactWithObjects = false;
        RightController.GetComponent<VRTK_Pointer>().interactWithObjects = false;
    }

    public virtual void EnteredSnapDropZone(object sender, SnapDropZoneEventArgs e) { }
    public virtual void ExitedSnapDropZone(object sender, SnapDropZoneEventArgs e) { }

    public virtual void EndActionsWithDraggedObj() { }

    public IEnumerator Answer(GameObject dropZone, bool right)
    {
        int childCount = dropZone.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            GameObject child = dropZone.transform.GetChild(i).gameObject;
            var ui = child.GetComponent<UIHelper>();

            if (ui != null)
            {
                ui.gameObject.SetActive(true);
                if (right)
                {
                    ui.Right();
                    GameManager.SetScore(1);
                }
                else
                {
                    ui.Wrong();
                    GameManager.SetScore(-1);
                }
                yield return new WaitForSeconds(1);
                ui.gameObject.SetActive(false);
            }
        }
    }

    public void DescriptionOn()
    {
        LeftController.GetComponent<VRTK_PolicyList>().identifiers.Add("Description");

        foreach (var item in objectsToDescription)
        {
            int childCount = item.transform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                GameObject child = item.transform.GetChild(i).gameObject;
                var ui = child.GetComponent<UIHelper>();

                if (ui != null)
                    ui.gameObject.SetActive(true);
            }
        }

        foreach (var item in DropZones)
        {
            item.GOCollider.enabled = false;
        }

        LeftController.GetComponent<VRTK_Pointer>().interactWithObjects = true;
        RightController.GetComponent<VRTK_Pointer>().interactWithObjects = true;
    }

    public IEnumerator Translate()
    {
        int i = 0;
        while (i < 50)
        {
            foreach (var item in PortableObjectsList)
                item.transform.position += new Vector3(0, -.005f, .015f);
            i++;
            yield return new WaitForSeconds(0.02f);
        }
    }

    protected bool ContainsList(List<List<GameObject>> list, List<GameObject> obj)
    {
        foreach (var item in list)
            if (obj.TrueForAll(o => item.Contains(o)))
                return true;
        return false;
    }

    protected bool ContainsDropZoneList(List<List<DropZoneBase>> list, List<DropZoneBase> obj)
    {
        foreach (var item in list)
            if (obj.TrueForAll(o => item.Contains(o)))
                return true;
        return false;
    }

    public void SetDescription(GameObject ObjectToDescription)
    {
        var jsonKeyScript = ObjectToDescription.GetComponent<JsonsKey>();
        string key;
        if (jsonKeyScript != null)
        {
            key = jsonKeyScript.Key;

            if (key != "")
            {
                if (scobeImageForTraining)
                {
                    if (key.Equals("boltikscob"))
                        scobeImageForTraining.SetActive(true);
                    else
                        scobeImageForTraining.SetActive(false);
                }
                Pair item = JsonParser.Items.Find(x => x.key == key);
                DescriptionText.text = item.value;
                SetSound(key);
            }
        }
    }

    public void SetDescription(string key)
    {
        if (key != "")
        {
            if (scobeImageForTraining)
            {
                if (key.Equals("boltikscob"))
                    scobeImageForTraining.SetActive(true);
                else
                    scobeImageForTraining.SetActive(false);
            }

            Pair item = JsonParser.Items.Find(x => x.key == key);
            if (item != null)
            {
                DescriptionText.text = item.value;
                SetSound(key);
            }
        }
    }

    public void SetSound(string fileName)
    {
        var sound = Resources.Load<AudioClip>("Audio/" + fileName);
        if (sound != null)
        {
            if (audioSourceHelpPanel.clip.name != fileName)
            {
                audioSourceHelpPanel.clip = sound;
                audioSourceHelpPanel.Play();
            }
        }
    }

    public void SetSoundCheckPanel(string fileName)
    {
        var sound = Resources.Load<AudioClip>("Audio/" + fileName);
        if (sound != null)
        {
            audioSourceCheckPanel.clip = sound;
            audioSourceCheckPanel.Play();
        }
    }
}