import os

def merge_files_in_directory(input_dir, output_file, extensions=None):
    """
    Đọc toàn bộ file trong input_dir và ghi nối lại thành output_file.

    :param input_dir: Đường dẫn tới thư mục chứa file
    :param output_file: Đường dẫn file kết quả
    :param extensions: List các phần mở rộng muốn lọc, ví dụ [".py", ".txt"], để None nếu lấy hết
    """
    with open(output_file, "w", encoding="utf-8") as outfile:
        for root, _, files in os.walk(input_dir):
            for filename in files:
                if extensions and not any(filename.endswith(ext) for ext in extensions):
                    continue  # bỏ qua file không đúng định dạng
                file_path = os.path.join(root, filename)
                try:
                    with open(file_path, "r", encoding="utf-8") as infile:
                        outfile.write(f"\n\n# ---- {file_path} ----\n\n")  # thêm tiêu đề để phân biệt file
                        outfile.write(infile.read())
                        print(f"Đã ghi {file_path}")
                except Exception as e:
                    print(f"Lỗi đọc {file_path}: {e}")

# Ví dụ dùng:
merge_files_in_directory(
    input_dir=r"C:\Users\phamt\source\repos\Online_Bookstore\Online_Bookstore\Services",
    output_file="merged_code.txt",
    extensions=[".cs", ".vue", ".js", ".ts", ".html", ".css"]  # tuỳ bạn muốn lọc loại file nào
)
