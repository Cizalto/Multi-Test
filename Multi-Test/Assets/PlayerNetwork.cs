using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private Library library;
    private GameObject projectilePrefab;

    void Start() {
        library = GameObject.Find("Library").GetComponent<Library>();
        projectilePrefab = library.GetPlayerProjectilePrefab();
    }
    
    // Update is called once per frame
    void Update() {
        if (!IsOwner) return;
        
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Z)) moveDir.y = 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        //Shoot
        if (Input.GetKeyDown(KeyCode.Space)) ShootServerRpc();
    }

    [ServerRpc]
    void ShootServerRpc() {
        ShootClientRpc();
    }

    [ClientRpc]
    void ShootClientRpc() {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
