using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class FollowCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    [ButtonMethod()]
    private string OnFindPlayerAndSet()
    {
        if (_camera == false)
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        var pc = FindPlayer();

        if (pc)
        {
            SetFollower(pc.transform);
            return "Player Controller 찾기 성공";
        }

        return "현재 Scene에서 다음 조건을 만족한 GameObject를 찾지 못했습니다.\n'Player' tag가 달린 GameObject\nPlayerController 컴포넌트를 소유한 GameObject";
    }

    private void Start()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();

        Debug.Assert(_camera);

        StartCoroutine(CoFindTarget());
    }

    [CanBeNull]
    private PlayerController FindPlayer()
    {
        var player = GameObject.FindWithTag("Player");

        if (player && player.TryGetComponent(out PlayerController tempPc))
        {
            return tempPc;
        }

        return null;
    }

    private void SetFollower(Transform target)
    {
        _camera.Follow = target;
        _camera.LookAt = target;
    }

    private IEnumerator CoFindTarget()
    {
        var wait = new WaitForEndOfFrame();
        PlayerController pc = null;

        while (true)
        {
            pc = FindPlayer();

            if (pc)
            {
                break;
            }

            yield return wait;
        }


        if (pc)
        {
            SetFollower(pc.transform);
        }
    }
}