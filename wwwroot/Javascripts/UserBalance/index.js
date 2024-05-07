$(document).ready(function() {
    //kiem tra xem user da co wallet hay chua.
    $.ajax({ 
        url: '/getuserbalance',
        type: "GET",
        contentType: "application/json",
        headers: {
            'Authorization' : 'Bearer ' + localStorage.getItem('jwtToken')
        },
        // neu co roi thi hien so du,
        success: (response) => {
            $("#userBalance").text('$'+response.balance)
        },
        //neu chua co thi hien nut tao
        error: () => {
            $("#createUserBalanceBtn").show();
            // Xử lý sự kiện khi click vào button "Create User Balance"
            $("#createUserBalanceBtn").click(function () {
                $("#createUserBalanceModal").modal("show");
            });


            // Xử lý sự kiện khi click nút "Create"
            $("#createUserBalance").click(function () {
                var balanceValue = $("#balanceInput").val();

                // Gửi AJAX request lên server
                $.ajax({
                    url: "/createuserbalance", // Đường dẫn đến API của bạn
                    type: "POST",
                    contentType: "application/json",
                    headers: {
                        "Authorization" : "Bearer " + localStorage.getItem("jwtToken")
                    },
                    data: JSON.stringify({ balance: balanceValue }),
                    success: function (response) {
                        // Xử lý phản hồi từ server nếu cần
                        $("#createUserBalanceBtn").hide();
                        console.log("User balance created successfully!");
                        toastr.options = {"positionClass": "toast-bottom-right"};
                        toastr.success("Tạo UserBalance thành công!");
                        setTimeout(function() {
                            location.reload();
                        }, 500);
                    },
                    error: function (xhr, status, error) {
                        // Xử lý lỗi nếu có
                        console.error("Error creating user balance:", xhr.responseJSON.message);
                        toastr.options = {"positionClass": "toast-bottom-right"};
                        toastr.error(xhr.responseJSON.message);
                    }
                });
            });
        }
    })
    
    
   
});
