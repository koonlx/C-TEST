using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//추가
using System.Net.Http;
using System.Net;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer; // 타이머 객체 생성

        public Form1()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            GetTmpDataRequest();
            GetLightDataRequest();
            GetDistanceDataRequest(); // 타이머마다 실행될 메소드
        }

        private void GetTmpDataRequest()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/tmp"); // 서버로 request
                request.Method = "GET"; // GET 요청
                request.ContentType = "application/json";  // 컨첸츠 타입 설정

                response = (HttpWebResponse)request.GetResponse(); // response 받아옴

                using (Stream responseStream = response.GetResponseStream()) // 응답 스트림 사용
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string responseText = reader.ReadToEnd(); // String으로 응답 값 받기
                    Console.WriteLine(responseText); // 콘솔 출력
                    this.textBox2.Text = responseText; // textBox2.Text에 응답값 할당
                }

                // 응답 닫기
                response.Close();
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Ex : " + ex);
                response.Close();
            }
        }

        private void GetLightDataRequest()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/light");
                request.Method = "GET";
                request.ContentType = "application/json";

                response = (HttpWebResponse)request.GetResponse();
                // 응답 스트림 읽기
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string responseText = reader.ReadToEnd();
                    Console.WriteLine(responseText);
                    this.textBox1.Text = responseText
                }

                // 응답 닫기
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex : " + ex);
            }
        }

        private void GetDistanceDataRequest()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/message/distance");
                request.Method = "GET";
                request.ContentType = "application/json";

                response = (HttpWebResponse)request.GetResponse();
                // 응답 스트림 읽기
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string responseText = reader.ReadToEnd();
                    Console.WriteLine(responseText);
                    this.textBox3.Text = responseText;
                }

                // 응답 닫기
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex : " + ex);
            }
        }

        private void conn_btn_Click(object sender, EventArgs e) // conn 버튼 클릭시 실행
        {
            
            
            String port =  this.comboBox1.Items[  this.comboBox1.SelectedIndex  ].ToString(); // 콤보박스에서 선택된 인덱스를 String 형태로 포트 변수에 저장
            Console.WriteLine("PORT : " + port); // 콘솔에 출력
            HttpWebRequest request=null;
            HttpWebResponse response = null;
            try
            {   
                request =  (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/connection/" + port);
                request.Method = "GET";
                request.ContentType = "application/json";
                //request.Timeout = 30 * 1000;

                response = (HttpWebResponse)request.GetResponse();
           
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("RESPONSE CODE : " + response.StatusCode);

                }
                bool connectionSuccessful = true;

                if (connectionSuccessful)
                {
                    // 연결에 성공한 경우 타이머를 시작.
                    timer = new System.Windows.Forms.Timer();
                    timer.Interval = 1000; // 1초마다 실행
                    timer.Tick += Timer_Tick; // Interval마다 Timer_Tick 실행
                    timer.Start(); // 타이머 시작
                }
                else
                {
                    // 연결에 실패한 경우 처리할 코드.
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ex : " + ex);
            }


        }

        private void led_on_btn_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/led/1");
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        private void led_off_btn_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:8080/arduino/led/0");
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        }
    }
}
