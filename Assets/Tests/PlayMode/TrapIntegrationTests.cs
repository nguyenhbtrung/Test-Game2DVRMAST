using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class TrapIntegrationTests
{
    //// Các hằng số cho “tiêu chuẩn” hitbox (bạn có thể thay đổi theo yêu cầu)
    //private const float allowedSizeRatioMin = 0.9f;
    //private const float allowedSizeRatioMax = 1.1f;
    //private const float allowedOffsetAbsRatio = 0.1f;

    //private const float allowedOffset = 0.04f;
    //[TestFixture]
    //public class T1
    //{
    //    [UnityTest]
    //    public IEnumerator Test2()
    //    {
    //        SceneManager.LoadScene(TestSettings.SceneIndex);
    //        yield return null;

    //        var player = GameObject.Find("NoDamage");
    //        var spriteRenderer = player.GetComponent<SpriteRenderer>();
    //        var sprite = spriteRenderer.sprite;
    //        Vector2 spriteSize = sprite.bounds.size;
    //        var collider = player.GetComponent<BoxCollider2D>();
    //        Vector2 colliderSize = collider.size;

    //        // Tính toán tỉ lệ kích thước
    //        float widthRatio = colliderSize.x / spriteSize.x;
    //        float heightRatio = colliderSize.y / spriteSize.y;

    //        Vector2 offset = collider.offset;
    //        float offsetRatioX = offset.x / spriteSize.x;
    //        float offsetRatioY = offset.y / spriteSize.y;
    //        Assert.Pass($"return new HitBoxConfig({widthRatio}f, {heightRatio}f, {offsetRatioX}f, {offsetRatioY}f);");
    //    }

    //    [UnityTest]
    //    public IEnumerator Test3()
    //    {
    //        SceneManager.LoadScene(TestSettings.SceneIndex);
    //        yield return null;
            

    //        GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
    //        Camera cam = GameObject.FindObjectOfType<Camera>();

    //        //player.transform.position = new Vector2(1.19f, 2.12f);
    //        yield return new WaitForSeconds(0.5f);

    //        Vector3 viewportPoint = cam.WorldToViewportPoint(player.transform.position);
    //        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z >= 0)
    //        {
    //            Assert.Pass("pass");
    //        }
    //        else
    //        {
    //            Assert.Fail("fail");
    //        }
    //    }



    //}


    //[TestFixture]
    //public class T2
    //{
    //    [UnityTest]
    //    [TestCase(1)]
    //    [TestCase(2)]
    //    public void Test5(int number)
    //    {
    //        Assert.Pass(number.ToString());
    //    }
    //}
    //[UnityTest]
    //public IEnumerator Test1()
    //{
    //    var logFilePath = TestLogger.GetLogFilePath();
    //    Assert.Pass($"Log file path: {logFilePath}");
    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator AllTraps_HaveTrapScript_And_BoxCollider2D()
    //{
    //    // Chờ 1 frame để đảm bảo các đối tượng trong scene được khởi tạo
    //    SceneManager.LoadScene(0);
    //    yield return null;

    //    // Tìm GameObject cha chứa tất cả traps
    //    GameObject trapsParent = GameObject.Find("bayax");
    //    Assert.IsNotNull(trapsParent, "Không tìm thấy GameObject 'bayax' trong scene.");

    //    // Duyệt qua tất cả các trap (con của Traps)
    //    foreach (Transform child in trapsParent.transform)
    //    {
    //        GameObject trap = child.gameObject;

    //        // Kiểm tra có component Trap không
    //        var trapScript = trap.GetComponent<Trap>();
    //        Assert.IsNotNull(trapScript, $"Chướng ngại vật '{trap.name}' không có script Trap.");

    //        // Kiểm tra có component BoxCollider2D không
    //        var boxCollider = trap.GetComponent<BoxCollider2D>();
    //        Assert.IsNotNull(boxCollider, $"Chướng ngại vật '{trap.name}' không có BoxCollider2D.");
    //    }

    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator Player_HitBox_IsValid()
    //{
    //    TestLogger.ClearLog(); 

    //    SceneManager.LoadScene(5);
    //    yield return null; 

    //    GameObject player = GameObject.Find("Player");
    //    Assert.IsNotNull(player, "Không tìm thấy GameObject 'Player' trong scene.");

    //    List<string> issues = new List<string>();

    //    var spriteRenderer = player.GetComponent<SpriteRenderer>();
    //    if (spriteRenderer == null)
    //    {
    //        string issue = $"Player không có SpriteRenderer.";
    //        issues.Add(issue);
    //    }

    //    var sprite = spriteRenderer.sprite;
    //    if (sprite == null)
    //    {
    //        string issue = $"Player không có sprite trong SpriteRenderer.";
    //        issues.Add(issue);
    //    }

    //    Vector2 spriteSize = sprite.bounds.size;

    //    // Kiểm tra BoxCollider2D
    //    var boxCollider = player.GetComponent<BoxCollider2D>();
    //    if (boxCollider == null)
    //    {
    //        string issue = $"Player không có BoxCollider2D.";
    //        issues.Add(issue);
    //    }


    //    HitBoxConfig playerHitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(HitBoxType.Player);

    //    Vector2 colliderSize = boxCollider.size;

    //    // Tính toán tỉ lệ kích thước
    //    float widthRatio = colliderSize.x / spriteSize.x;
    //    float heightRatio = colliderSize.y / spriteSize.y;

    //    if (Mathf.Abs(widthRatio - playerHitBoxConfig.sizeRatio.x) > allowedOffset ||
    //        Mathf.Abs(heightRatio - playerHitBoxConfig.sizeRatio.y) > allowedOffset)
    //    {
    //        string issue = $"Player có kích thước HitBox không hợp lệ.";
    //        issues.Add(issue);

    //    }

    //    Vector2 offset = boxCollider.offset;
    //    float offsetRatioX = offset.x / spriteSize.x;
    //    float offsetRatioY = offset.y / spriteSize.y;

    //    if (Mathf.Abs(offsetRatioX - playerHitBoxConfig.offsetRatio.x) > allowedOffset ||
    //        Mathf.Abs(offsetRatioY - playerHitBoxConfig.offsetRatio.y) > allowedOffset)
    //    {
    //        string issue = $"Player có offset HitBox không hợp lệ.";
    //        issues.Add(issue);
    //    }

    //    if (issues.Count > 0)
    //    {
    //        string errorMessage = "Phát hiện lỗi Player hitbox\n" + string.Join("\n", issues);
    //        TestLogger.Log(errorMessage);
    //        Assert.Fail(errorMessage);
    //    }
    //    else
    //    {
    //        string successMessage = "Player hitbox hợp lệ.";
    //        TestLogger.Log(successMessage);
    //        Assert.Pass(successMessage);
    //    }

    //    yield return null;

    //}

    //[UnityTest]
    //public IEnumerator FinishPoint_HitBox_IsValid()
    //{
    //    TestLogger.ClearLog();

    //    SceneManager.LoadScene(5);
    //    yield return null;

    //    GameObject player = GameObject.Find("End (Pressed) (64x64)_0");
    //    Assert.IsNotNull(player, "Không tìm thấy GameObject 'End (Pressed) (64x64)_0' trong scene.");

    //    List<string> issues = new List<string>();

    //    var spriteRenderer = player.GetComponent<SpriteRenderer>();
    //    if (spriteRenderer == null)
    //    {
    //        string issue = $"End (Pressed) (64x64)_0 không có SpriteRenderer.";
    //        issues.Add(issue);
    //    }

    //    var sprite = spriteRenderer.sprite;
    //    if (sprite == null)
    //    {
    //        string issue = $"End (Pressed) (64x64)_0 không có sprite trong SpriteRenderer.";
    //        issues.Add(issue);
    //    }

    //    Vector2 spriteSize = sprite.bounds.size;

    //    var boxCollider = player.GetComponent<BoxCollider2D>();
    //    if (boxCollider == null)
    //    {
    //        string issue = $"End (Pressed) (64x64)_0 không có BoxCollider2D.";
    //        issues.Add(issue);
    //    }


    //    HitBoxConfig playerHitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(HitBoxType.FinishPoint);

    //    Vector2 colliderSize = boxCollider.size;

    //    // Tính toán tỉ lệ kích thước
    //    float widthRatio = colliderSize.x / spriteSize.x;
    //    float heightRatio = colliderSize.y / spriteSize.y;

    //    if (Mathf.Abs(widthRatio - playerHitBoxConfig.sizeRatio.x) > allowedOffset ||
    //        Mathf.Abs(heightRatio - playerHitBoxConfig.sizeRatio.y) > allowedOffset)
    //    {
    //        string issue = $"End (Pressed) (64x64)_0 có kích thước HitBox không hợp lệ.";
    //        issues.Add(issue);

    //    }

    //    Vector2 offset = boxCollider.offset;
    //    float offsetRatioX = offset.x / spriteSize.x;
    //    float offsetRatioY = offset.y / spriteSize.y;

    //    if (Mathf.Abs(offsetRatioX - playerHitBoxConfig.offsetRatio.x) > allowedOffset ||
    //        Mathf.Abs(offsetRatioY - playerHitBoxConfig.offsetRatio.y) > allowedOffset)
    //    {
    //        string issue = $"End (Pressed) (64x64)_0 có offset HitBox không hợp lệ.";
    //        issues.Add(issue);
    //    }

    //    if (issues.Count > 0)
    //    {
    //        string errorMessage = "Phát hiện lỗi End (Pressed) (64x64)_0 hitbox\n" + string.Join("\n", issues);
    //        TestLogger.Log(errorMessage);
    //        Assert.Fail(errorMessage);
    //    }
    //    else
    //    {
    //        string successMessage = "End (Pressed) (64x64)_0 hitbox hợp lệ.";
    //        TestLogger.Log(successMessage);
    //        Assert.Pass(successMessage);
    //    }

    //    yield return null;

    //}


    //[UnityTest]
    //public IEnumerator Trap_HitboxParameters_AreWithinAllowedRange()
    //{
    //    TestLogger.ClearLog(); // Xóa log trước khi chạy test

    //    SceneManager.LoadScene(5);
    //    yield return null; // Chờ 1 frame

    //    GameObject trapsParent = GameObject.Find("bayax");
    //    Assert.IsNotNull(trapsParent, "Không tìm thấy GameObject 'bayax' trong scene.");

    //    List<string> trapsWithIssues = new List<string>(); // Danh sách lưu các trap có vấn đề

    //    foreach (Transform child in trapsParent.transform)
    //    {
    //        GameObject trap = child.gameObject;

    //        // Kiểm tra SpriteRenderer
    //        var spriteRenderer = trap.GetComponent<SpriteRenderer>();
    //        if (spriteRenderer == null)
    //        {
    //            string issue = $"Chướng ngại vật '{trap.name}' không có SpriteRenderer.";
    //            trapsWithIssues.Add(issue);
    //            TestLogger.Log(issue);
    //            continue;
    //        }

    //        var sprite = spriteRenderer.sprite;
    //        if (sprite == null)
    //        {
    //            string issue = $"Chướng ngại vật '{trap.name}' không có sprite trong SpriteRenderer.";
    //            trapsWithIssues.Add(issue);
    //            TestLogger.Log(issue);
    //            continue;
    //        }

    //        // Lấy kích thước của sprite (trong local space)
    //        Vector2 spriteSize = sprite.bounds.size;

    //        // Kiểm tra BoxCollider2D
    //        var boxCollider = trap.GetComponent<BoxCollider2D>();
    //        if (boxCollider == null)
    //        {
    //            string issue = $"Chướng ngại vật '{trap.name}' không có BoxCollider2D.";
    //            trapsWithIssues.Add(issue);
    //            TestLogger.Log(issue);
    //            continue;
    //        }

    //        HitBoxConfig hitBoxConfig = HitBoxConfigManager.GetHitBoxConfig(GetTrapHitboxType(trap.name));

    //        Vector2 colliderSize = boxCollider.size;

    //        // Tính toán tỉ lệ kích thước
    //        float widthRatio = colliderSize.x / spriteSize.x;
    //        float heightRatio = colliderSize.y / spriteSize.y;
    //        bool sizeIssue = false;

    //        if (Mathf.Abs(widthRatio - hitBoxConfig.sizeRatio.x) > allowedOffset ||
    //            Mathf.Abs(heightRatio - hitBoxConfig.sizeRatio.y) > allowedOffset)
    //        {
    //            sizeIssue = true;
    //        }

    //        // Tính toán tỉ lệ offset
    //        Vector2 offset = boxCollider.offset;
    //        float offsetRatioX = offset.x / spriteSize.x;
    //        float offsetRatioY = offset.y / spriteSize.y;
    //        bool offsetIssue = false;

    //        if (Mathf.Abs(offsetRatioX - hitBoxConfig.offsetRatio.x) > allowedOffset ||
    //            Mathf.Abs(offsetRatioY - hitBoxConfig.offsetRatio.y) > allowedOffset)
    //        {
    //            offsetIssue = true;
    //        }

    //        // Ghi lại các vấn đề nếu có
    //        if (sizeIssue || offsetIssue)
    //        {
    //            string issues = $"Chướng ngại vật '{trap.name}' có vấn đề:";
    //            if (sizeIssue)
    //            {
    //                issues += $" Tỉ lệ kích thước không hợp lệ (widthRatio = {widthRatio:F2}, heightRatio = {heightRatio:F2}).";
    //            }
    //            if (offsetIssue)
    //            {
    //                issues += $" Tỉ lệ offset không hợp lệ (offsetRatioX = {offsetRatioX:F2}, offsetRatioY = {offsetRatioY:F2}).";
    //            }
    //            trapsWithIssues.Add(issues);
    //        }
    //    }

    //    // Kiểm tra và báo cáo kết quả
    //    if (trapsWithIssues.Count > 0)
    //    {
    //        string errorMessage = "Phát hiện các chướng ngại vật không hợp lệ:\n" + string.Join("\n", trapsWithIssues);
    //        TestLogger.Log(errorMessage);
    //        Assert.Fail(errorMessage);
    //    }
    //    else
    //    {
    //        string successMessage = "Tất cả các chướng ngại vật đều hợp lệ.";
    //        TestLogger.Log(successMessage);
    //        Assert.Pass(successMessage);
    //    }

    //    yield return null;
    //}

    //public HitBoxType GetTrapHitboxType(string trapName)
    //{
    //    if (trapName.StartsWith("On (38x38)_0"))
    //    {
    //        return HitBoxType.OnTrap;
    //    }
    //    else if (trapName.StartsWith("Idle"))
    //    {
    //        return HitBoxType.IdleTrap;
    //    }
    //    else if (trapName.StartsWith("Blink (54x52)_0"))
    //    {
    //        return HitBoxType.BlinkTrap;
    //    }
    //    else
    //    {
    //        throw new ArgumentException("Unknown trap name");
    //    }
    //}


}

//public class T3Base
//{
//    [UnitySetUp]
//    public IEnumerator BaseSetUp()
//    {
//        Debug.Log("Set up base");
//        yield return null;
//    }
//}

//public class T31 : T3Base
//{
//    [UnitySetUp]
//    public IEnumerator SetUp()
//    {
//        Debug.Log("Set up t31");
//        yield return null;
//    }

//    [UnityTest]
//    public IEnumerator Test1()
//    {
//        Debug.Log("test t31");
//        yield return null;
//    }

//    [UnityTearDown]
//    public IEnumerator TearDown()
//    {
//        Debug.Log("TearDown t31");
//        yield return null;
//    }
//}

//public class T32 : T3Base
//{
//    [UnitySetUp]
//    public IEnumerator SetUp()
//    {
//        Debug.Log("Set up t32");
//        yield return null;
//    }

//    [UnityTest]
//    public IEnumerator Test1()
//    {
//        Debug.Log("test t32");
//        yield return null;
//    }

//}
