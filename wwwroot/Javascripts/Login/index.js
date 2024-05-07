$(document).ready(function() {
    if (localStorage.getItem('redirectToLogin')) {
        toastr.options = {"positionClass": "toast-bottom-right"};
        toastr.error("Hãy đăng nhập!");
        // Sau khi hiển thị toastr, xóa trạng thái từ localStorage để không hiển thị lại toastr khi trang được tải lại
        localStorage.removeItem('redirectToLogin');
    }
    function saveUserDataToLocalStorage(token) {
        // Decode the JWT token to extract user information
        var decodedToken = decodeJWTToken(token);
        if (decodedToken) {
            localStorage.setItem('jwtToken', token);
            localStorage.setItem('username', decodedToken.email);
        }
    }
    function decodeJWTToken(token) {
        try {
            var base64Url = token.split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var decodedToken = JSON.parse(atob(base64));
            return decodedToken;
        } catch (error) {
            console.error('Error decoding JWT token:', error);
            return null;
        }
    }
    // Bắt sự kiện click trên nút "Sign in"
    $("#LoginButton").click(function() {
        // Lấy giá trị của email và password từ các trường input
        var email = $("#inputEmail").val();
        var password = $("#inputPassword").val();

        // Kiểm tra xem email và password có được nhập vào không
        if (email && password) {
            // Gửi yêu cầu đăng nhập đến server
            $.ajax({
                type: "POST",
                url: "/api/user/login",
                contentType: "application/json",
                data: JSON.stringify({
                    email: email,
                    password: password
                }),
                success: function(response) {
                    // Xử lý phản hồi từ server sau khi đăng nhập thành công
                    console.log("Login successful:", response);
                    // Lưu data vào local storage
                    saveUserDataToLocalStorage(response.token);
                    window.location.href = "/home";
              
                },
                error: function(xhr, status, error) {
                    // Xử lý lỗi nếu có
                    console.error("Login error:", error);
                    toastr.options = {"positionClass": "toast-bottom-right"};
                    toastr.error("Sai tài khoản/ mật khẩu, hãy kiểm tra lại");
                }
            });
        } else {
            // Hiển thị thông báo lỗi nếu email hoặc password trống
            toastr.options = {"positionClass": "toast-bottom-right"};
            toastr.error("Hãy điền đầy đủ các trường thông tin để đăng nhập.");
        }
    });
});
