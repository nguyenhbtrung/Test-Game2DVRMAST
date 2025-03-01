using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HitboxBehaviorTests
{
    public class FinishPointBehaviorTests : HitboxBehaviorTestsBase
    {
        [UnityTest]
        public IEnumerator TestPlayerCollisionFinishPoint()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionFinishPoint);
            TestLogger.Log(message, logFileName);

            GameObject finishPoint = GameObject.Find(TestHelper.FINISH_POINT_NAME);
            Assert.IsNotNull(finishPoint, "Không tìm thấy FinishPoint trong scene.");
            Win win = finishPoint.GetComponent<Win>();

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);

            player.transform.position = finishPoint.transform.position;
            yield return null;

            if (!win.gameWinIU.activeInHierarchy)
            {
                string err = "Màn hình gameWin không hiển thị khi Player va chạm với FinishPoint.";
                TestLogger.Log(err, logFileName);
                Assert.Fail(err);
            }

            string successMessage = $"{nameof(TestPlayerCollisionFinishPoint)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
            Time.timeScale = 1;

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Time.timeScale = 1;
            yield return null;
        }
    }
}

