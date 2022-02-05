using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考ページ　https://dkrevel.com/makegame-beginner/make-2d-action-shot-enemy/
/*真っすぐ横に弾を飛ばすと、一定距離で球が消えるため不自然に途中で弾が消える
 主人公が一定距離に近づいたら、動き出すという仕組みを作ればよいが面倒くさいので、
弾を前方下に発射することで不自然さを解消。このスクリプトのままだと、
弾が上に発射されるので、オブジェクトを90度回転させ、Rigidbodyで重力を付ける。
 */

public class EnemyAttack : MonoBehaviour {
    [Header("スピード")] public float speed = 3.0f;
    [Header("最大移動距離")] public float maxDistance = 100.0f;
    private Rigidbody2D rb;
    private Vector3 defaultPos;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            Debug.Log("設定が足りません");
            Destroy(this.gameObject);
        }
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
            float d = Vector3.Distance(transform.position, defaultPos);

            //最大移動距離を超えている
            if (d > maxDistance) {
                Destroy(this.gameObject);
            } else {
                rb.MovePosition(transform.position += transform.up * Time.deltaTime * speed);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(this.gameObject);
    }
}
