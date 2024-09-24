using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml;

namespace Lab_4
{
    public delegate int SoSanh(object sv1, object sv2);
    class QuanLySinhVien
    {
        public List<SinhVien> DanhSach;
       
        public QuanLySinhVien()
        {
            DanhSach = new List<SinhVien>();
        }
        // Thêm một sinh viên vào danh sách
        public void Them(SinhVien sv)
        {
            this.DanhSach.Add(sv);
        }
        public SinhVien this[int index]
        {
            get { return DanhSach[index]; }
            set { DanhSach[index] = value; }
        }
        //Xóa các obj trong danh sách nếu thỏa điều kiện so sánh
        public void Xoa(object obj, SoSanh ss)
        {
            int i = DanhSach.Count - 1;
            for (; i >= 0; i--)
                if (ss(obj, this[i]) == 0)
                    this.DanhSach.RemoveAt(i);
        }
        //Tìm một sinh viên trong danh sách thỏa điều kiện so sánh
        public SinhVien Tim(object obj, SoSanh ss)
        {
            SinhVien svresult = null;
            foreach (SinhVien sv in DanhSach)
                if (ss(obj, sv) == 0)
                {
                    svresult = sv;
                    break;
                }
            return svresult;
        }
        //Tìm một sinh viên trong danh sách thỏa điều kiện so sánh,

        public bool Sua(SinhVien svsua, object obj, SoSanh ss)
        {
            int i, count;
            bool kq = false;
            count = this.DanhSach.Count - 1;
            for (i = 0; i < count; i++)
                if (ss(obj, this[i]) == 0)
                {
                    this[i] = svsua;
                    kq = true;
                    break;
                }
            return kq;
        }

        // Hàm đọc danh sách sinh viên từ tập tin txt
        public void DocTuFile()
        {
            string filename = "DanhSachSV.txt", t;
            string[] s;
            SinhVien sv;
            StreamReader sr = new StreamReader(new FileStream(filename, FileMode.Open));
            while ((t = sr.ReadLine()) != null)
            {
                s = t.Split('*');
                sv = new SinhVien();
                sv.MaSo = s[0];
                sv.HoTen = s[1];
                sv.GioiTinh = false;
                if (s[2] == "1")
                    sv.GioiTinh = true;
                sv.NgaySinh = DateTime.Parse(s[3]);
                sv.Lop = s[4];
                sv.DienThoai = s[5];
                sv.Email = s[6];
                sv.DiaChi = s[7];
                sv.Hinh = s[8];
                this.Them(sv);
            }
        }

        /// <summary>
        /// Phương thức đọc tập tin JSON
        /// </summary>
        /// <param name="Path">Đường dẫn tập tin</param>
        /// <returns> Danh sách các đối tượng từ tập tin JSON</returns>
        public void LoadJSON(string Path)
        {

            // Đối tượng đọc tập tin
            StreamReader r = new StreamReader(Path);
            string json = r.ReadToEnd(); // Đọc hết
                                         // Chuyển về thành mảng các đối tượng
            var array = (JObject)JsonConvert.DeserializeObject(json);
            // Lấy đối tượng sinhvien
            var students = array["sinhvien"].Children();
            foreach (var item in students) // Duyệt mảng
            {
                // Lấy các thành phần
                string mssv = item["MSSV"].Value<string>();
                string hoten = item["HoTen"].Value<string>();
                DateTime NgaySinh = item["NgaySinh"].Value<DateTime>();
                string Email = item["Email"].Value<string>();
                string DiaChi = item["DiaChi"].Value<string>();
                string Lop = item["Lop"].Value<string>();
                string Hinh = item["Hinh"].Value<string>();
                bool GioiTinh = item["GioiTinh"].Value<bool>();
                string DienThoai = item["DienThoai"].Value<string>();
                // Chuyển vào đối tượng StudentInfo
                SinhVien info = new SinhVien(mssv, hoten, GioiTinh, NgaySinh, Lop,
               DiaChi, Email, Hinh , DienThoai);
                this.Them(info);// Thêm vào danh sách
            }
        }


    }

}
