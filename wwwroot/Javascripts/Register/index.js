$(document).ready(function() {
    function validateEmail(email) {
        const regex = /\S+@\S+\.\S+/;
        return regex.test(email);
    }

    $("#btnSignUp").click(function() {
        // Lấy giá trị của email và password từ các trường input
        var email = $("#emailInput").val();
        var password = $("#passwordInput").val();
        var firstName = $("#firstNameInput").val();
        var lastName = $("#lastNameInput").val();

        // Kiểm tra xem email và password có được nhập vào không
        if (email && password && firstName && lastName) {
            if (!validateEmail(email)) {
                toastr.options = {"positionClass": "toast-bottom-right"};
                toastr.error("Email không hợp lệ.");
                return; // Dừng việc gửi yêu cầu nếu email không hợp lệ
            }

            // Disable nút Sign up để ngăn ngừa người dùng gửi nhiều yêu cầu
            $("#btnSignUp").prop("disabled", true);

            // Gửi yêu cầu đăng ký đến server
            $.ajax({
                type: "POST",
                url: "/api/user/register",
                contentType: "application/json",
                data: JSON.stringify({
                    email: email,
                    password: password,
                    firstName: firstName,
                    lastName: lastName
                }),
                success: function(response) {
                    console.log("Register successful:", response);
                    toastr.options = {"positionClass": "toast-bottom-right"};
                    toastr.success("Đăng kí thành công!");
                    // Chuyển hướng đến trang Login
                    setTimeout(function() {
                        window.location.href = "/login";
                    }, 1000);
                },
                error: function(xhr, status, error) {
                    // Xử lý lỗi nếu có
                    console.log("Register error:", xhr.responseJSON.message);
                    toastr.options = {"positionClass": "toast-bottom-right", "timeOut": 3000};
                    toastr.error(xhr.responseJSON.message || "Đã xảy ra lỗi khi đăng kí.");
                },
                complete: function() {
                    // Enable lại nút Sign up sau khi xử lý hoàn tất
                    $("#btnSignUp").prop("disabled", false);
                }
            });
        } else {
            // Hiển thị thông báo lỗi nếu email hoặc password trống
            toastr.options = {"positionClass": "toast-bottom-right"};
            toastr.error("Hãy điền đầy đủ các trường thông tin để đăng kí.");
        }
    });
});
