using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Vector3 FollowDistance = new Vector3(-10,20,-10);
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentPlayer)
            Follow(GameManager.Instance.CurrentPlayer.transform);
    }


    void Follow(Transform target)
    {
        transform.position = FollowDistance + target.position;
        transform.LookAt(target);
    }

    void StopAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
