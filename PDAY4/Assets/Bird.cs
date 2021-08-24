using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //�@���̃v���n�u�i�[�z��
    public GameObject[] birdPrefabs;

    //�A������p�̒��̋���
    const float BirdDistance = 3.5f;

    //�Œ�A����
    const int MinChain = 3;

    //�N���b�N���ꂽ�����i�[
    private GameObject firstBird;
    private GameObject lastBird;
    private string currentName;
    List<GameObject> removableBirdList = new List<GameObject>();

    //�A�����킩��悤�Ƀ��C��������
    public GameObject lineObj;
    List<GameObject> lineBirdList = new List<GameObject>();

    public GameObject scoreGUI;
    private int point = 100;

    // Start is called before the first frame update
    void Start()
    {
        TouchManager.Began += (info) =>
        {
           
            //�N���b�N�n�_�Ńq�b�g���Ă���I�u�W�F�N�g���擾
            RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObj = hit.collider.gameObject;
                //�q�b�g�����I�u�W�F�N�g�^�O��bird��������
                if (hitObj.tag == "bird")
                {
                    firstBird = hitObj;
                    lastBird = hitObj;
                    currentName = hitObj.name;
                    removableBirdList = new List<GameObject>();
                    lineBirdList = new List<GameObject>();
                    PushToBirdList(hitObj);
                }
            }
        };
        TouchManager.Moved += (info) =>
        {
            if (firstBird == null)
            {
                return;
            }
            //�N���b�N�n�_�Ńq�b�g���Ă���I�u�W�F�N�g���擾
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObj = hit.collider.gameObject;
                //�q�b�g�����I�u�W�F�N�g��Tag��bird�����A���O���ꏏ
                //�Ō��hit�����I�u�W�F�N�g�ƈႤ�A���X�g�ɂ͓����Ă��Ȃ�
                if (hitObj.tag == "bird" && hitObj.name == currentName
                && lastBird != hitObj && 0 > removableBirdList.IndexOf(hitObj))
                {
                    float distance = Vector2.Distance(hitObj.transform.position,lastBird.transform.position);
                    if(distance > BirdDistance)
                    {
                        return;
                    }
                    PushToLineList(hitObj, lastBird);
                    lastBird = hitObj;
                    PushToBirdList(hitObj);
                }  
            }
        };
        TouchManager.Ended += (info) =>
        {
            int count = removableBirdList.Count;

            if(count >= MinChain)
            {
                //���X�g�Ɋi�[����Ă��钹������
                foreach (GameObject obj in removableBirdList)
                {
                    Destroy(obj);
                }
                StartCoroutine(DropBird(count));
                //scoreGUI.SendMessage("AddPoint", point * remove_cnt);
            }

            foreach (GameObject obj in removableBirdList)
            {
                ChangeColor(obj, 1.0f);
            }
            foreach (GameObject obj in lineBirdList)
            {
                Destroy(obj);
            }
            removableBirdList = new List<GameObject>();
            firstBird = null;
            lastBird = null;
        };
        StartCoroutine(DropBird(50));
    }
    private void PushToLineList(GameObject lastObj,GameObject hitObj)
    {
        GameObject line = (GameObject)Instantiate(lineObj);
        LineRenderer renderer = line.GetComponent<LineRenderer>();
        //���̑���
        renderer.startWidth = 0.1f;
        renderer.endWidth = 0.1f;
        //���_�̐�
        renderer.positionCount = 2;
        //���_��ݒ�
        renderer.SetPosition(0,new Vector3(lastObj.transform.position.x, lastObj.transform.position.y, -1.0f));
        renderer.SetPosition(1, new Vector3(hitObj.transform.position.x, hitObj.transform.position.y, -1.0f));
        lineBirdList.Add(line);
    }


    private void PushToBirdList(GameObject obj)
    {
        removableBirdList.Add(obj);
        ChangeColor(obj, 0.5f);
    }

    private void ChangeColor(GameObject obj,float transparency)
    {
        SpriteRenderer birdTexture = obj.GetComponent<SpriteRenderer>();
        birdTexture.color = new Color(birdTexture.color.r, birdTexture.color.g, birdTexture.color.b, transparency);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //�@�w�肳�ꂽ�������𔭐�������R���[�`��
    IEnumerator DropBird(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //�@�����_���ŏo���ʒu���쐬
            Vector2 pos = new Vector2(Random.Range(-7f, 7f), 16f);

            //�@�o�����钹��ID���쐬
            int id = Random.Range(0, birdPrefabs.Length);

            //�@���𔭐�������
            GameObject bird = (GameObject) Instantiate(birdPrefabs[id],
                pos,
                Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward));

            //�@�쐬�������̖��O��ID���g���Ă��Ȃ���
            bird.name = "Bird" + id;

            // 0.05�b�҂��Ď��̏�����
            yield return new WaitForSeconds(0.05f);
        }
    }
}
