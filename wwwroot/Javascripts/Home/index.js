$(document).ready(function () {
    // Function to get user name from Local Storage
    function getUsernameFromLocalStorage() {
        return localStorage.getItem('username');
    }
    $("#homeMenuBtn").show();

    // Function to remove JWT Token and user name from Local Storage
    function removeUserDataFromLocalStorage() {
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('username');
    }


    function updateUI() {
        var username = getUsernameFromLocalStorage();
        if (username) {
            // User is logged in
            $("#usernameDisplay").text("Hi, " + username); // Display username on menu
            $("#logoutButton").show(); // Show logout button
            $("#loginButton").hide(); // Hide login button
        } else {
            // User is not logged in
            $("#usernameDisplay").text(''); // Clear username display
            $("#logoutButton").hide(); // Hide logout button
            $("#loginButton").show(); // Show login button
        }
    }

    updateUI();
    $("#logoutButton").click(function () {
        removeUserDataFromLocalStorage(); // Remove user data from Local Storage
        updateUI(); // Update UI
        // Perform any additional logout actions if needed
        window.location.href = "/login";
    });  
    $("#logoutATag").click(function () {
        $("#logoutButton").click();
    });
    /*
        $.ajax({
            url: "/getallprocedures",
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
            },
            success: (data, response) => {
                var statusTag = $("#statusLog");
    
                statusTag.text(data.toString());
            },
            error: (jqXHR, textStatus, errorThrown) => {
                var statusTag = $("#statusLog");
    
                statusTag.text(textStatus);
                console.log("error: ", textStatus, errorThrown)
            }
    
        });
    */
    function displayData(data) {
        $('#categoryGrid').empty();

        // Calculate the number of columns based on the screen width
        var screenWidth = $(window).width();

        var numColumns = Math.floor(screenWidth / 250); // Adjust the number as needed

        // Calculate the column width based on the number of columns 
        var columnWidth = 100 / numColumns;

        // Duyệt qua mảng dữ liệu và tạo các phần tử HTML
        data.forEach(function(item) {
            var btnClass = (item.spent > item.budget) ? 'btn-danger font-weight-bold' : 'btn-secondary';
            var bgClass = (item.spent > item.budget) ? 'bg-warning' : '';
            var vibrationClass = (item.spent > item.budget) ? 'card-vibrate' : '';
            var overAndPercent = (item.spent > item.budget) ? ' -$' + (item.spent - item.budget) + ' (-'+ ((item.spent-item.budget) / item.budget)*100 +'%)' : '';
            var overBudgetText = (item.spent > item.budget) ? '<i class="fas fa-bell"></i> Over budget:' : '';
            var truncatedName = item.name.length > 9 ? item.name.substring(0, 9) + '..' : item.name;

            var cardHtml = '<div class="col-md-3 mb-4">';
            cardHtml += '<div class="card '+vibrationClass+' ' + bgClass + '" style="border-radius: 10px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); transition: transform 0.3s;">';
            cardHtml += '<div class="card-body p-4">';
            cardHtml += '<h5 class="card-title mb-3 text-center">';
            cardHtml += '<button class="btn btn-lg ' + btnClass + ' shadow-lg fw-bold" style="border-radius: 10px; padding: 10px 20px;" data-toggle="tooltip" data-placement="top" title="#' + item.name + '">';
            cardHtml += '#' + truncatedName + '</button></h5>';
            cardHtml += '<p class="card-text mb-1 text-center"><strong>Budget:</strong> <span class="fw-bold" style="font-size: 1.3rem;">$' + item.budget + '</span></p>';
            cardHtml += '<p class="card-text mb-3 text-center"><strong>Spent:</strong> <span class="fw-bold" style="font-size: 1.3rem;">$' + item.spent + '</span></p>';
            cardHtml += '<p class="card-text mb-3 text-center"><strong>'+overBudgetText+'</strong> <span class="fw-bold" style="color:red; font-size: 1.3rem;">' + overAndPercent + '</span></p>';
            cardHtml += '</div></div></div>';

            $('#categoryGrid').append(cardHtml);
        });
// Enable Bootstrap tooltips
        $('[data-toggle="tooltip"]').tooltip();
    }
    $.ajax({
        url: '/api/category',
        method: 'GET',
        headers:{
            "Authorization": "Bearer "+ localStorage.getItem("jwtToken")
        },
        success: function(response) {
            // Nếu lấy dữ liệu thành công, hiển thị lên giao diện
            displayData(response);
        },
        error: function(xhr, status, error) {
            // Xử lý lỗi nếu có
            console.error(status, error);
        }
    });
    $.ajax({
        url: "/transaction/getlist",
        contentType: "application/json",
        method: "GET",
        headers: {
            "Authorization": "Bearer "+ localStorage.getItem("jwtToken")
        },
        success: (data, response) => {
            console.log(data);
        },
        error: (xhr, data, response) => {
            console.log(xhr.message);
        }
    })

/*
    $("#transactionTable").DataTable();
*/
    var table = $('#transactionTable').DataTable({
        "ajax": {
            "processing": true,
            "serverSide": true,
            "url": "/transaction/getlist/",
            "type": "GET",
            "contentType": "application/json",
            "headers": {
                "Authorization": "Bearer " + localStorage.getItem("jwtToken")
            },
            "dataSrc" : (d)=>{
                return d;
            }
        },
        "columns": [
            {
                width: "2%",
                className: "dt-start",
                "title": "Category",
                "data": "categoryName",
                "render": (data,type,row)=>{
                    return data != null 
                        ?`<button class="btn btn-md fw-bold btn-light">
                            <i class="fas fa-hashtag"></i>`+data+`
                         </button>`
                        : ` <button class="btn btn-md fw-bold btn-secondary text-light"><i class="fas fa-hashtag"></i>No Tag</button> `
                }
            },
            {
                width: "1%",
                "title": "Title",
                "data": "title"
            },
            {
                width: "1%",
                className: "dt-center",
                "title": "Date",
                "data": "date",
                "render": (data,type,row)=>{
                    return moment(data).format('HH:mm DD/MM/YYYY')
                }
            },
            {
                width: "1%",
                className: "dt-center",
                "title": "Type",
                "data": "type",
                "render": (data,type,row)=>{
                    return data == 0 
                        ? ` <button class="btn btn-md btn-success fw-bold text-light shadow">Income</button> ` 
                        : `<button class="btn btn-md btn-danger fw-bold text-light shadow">Outcome</button> `
                }
            },
           
            {   
                width: "1%",
                "title": "Amount",
                "data": "amount",
                "render": function(data, type, row) {
                    if (row.type == 0) {
                        return `+` + parseFloat(data).toFixed(2) + `$`;
                    } else {
                        return `-` + parseFloat(data).toFixed(2) +`$`;
                    }
                    return parseFloat(data).toFixed(2)+`$`;
                }},
            {
                width: "1%",
                "title": "Description",
                "data": "description"
            },
            {
                width: "1%",
                "title": "Email",
                "data": "email"
            }
        ]
    });
    

    $('#categorySelect').change(function () {
        if($(this).val() != "00000000-0000-0000-0000-000000000000"){
            $("#typeDiv").hide(); // Ẩn phần tử nếu giá trị của select box là "00000000-0000-0000-0000-000000000000"
        } else {
            $("#typeDiv").show(); // Hiển thị phần tử nếu giá trị của select box không phải là "00000000-0000-0000-0000-000000000000"
        }
    });
    
    $.ajax({
        url: '/api/category/selected', // Thay đổi đường dẫn API tùy thuộc vào ứng dụng của bạn
        type: 'GET',
        headers:{
            "Authorization":"Bearer "+localStorage.getItem("jwtToken")
        },
        success: function(categories) {
            categories.forEach(function(category) {
                console.log(category);
                $('#categorySelect').append($('<option>', {
                    value: category.id,
                    text: category.name
                }));
            });
        },
        error: function(error) {
            console.error('Error fetching categories:', error);
        }
    });
    $("#createCategoryBtn").click(()=>{
        $("#createCategoryModal").modal("show");
        $("#createCategory").click(()=>{
            var nameInput = $("#nameInput").val();
            var budgetInput = parseInt($('#budgetInput').val());
            var userBalanceIdInput = "00000000-0000-0000-0000-000000000000";
           
            $.ajax({
                type: "POST",
                url: "/api/category",
                contentType: "application/json",
                headers:{
                    "Authorization":"Bearer "+ localStorage.getItem("jwtToken")
                },
                data: JSON.stringify({
                    name: nameInput,
                    budget: budgetInput,
                    userBalanceId: userBalanceIdInput
                }),
                success: function(response) {
                    console.log("Create successful:", response);
                    toastr.options = {"positionClass": "toast-bottom-right", "timeOut": 3000};
                    toastr.success("Tạo category thành công!");
                    setTimeout(function() {
                        window.location.href = "/home";
                    }, 1000);
                },
                error: function(xhr, status, error) {
                    // Xử lý lỗi nếu có
                    if(xhr.status === 500) {
                        toastr.options = {"positionClass": "toast-bottom-right", "timeOut": 3000};
                        toastr.error(xhr.responseText);
                    } else {
                        console.log('Đã xảy ra lỗi: ' + xhr.responseJSON.message);
                        toastr.options = {"positionClass": "toast-bottom-right", "timeOut": 3000};
                        toastr.error("Tạo category lỗi:", xhr.responseJSON.message);
                    }
                }
            })
            
        })
        
        
    })
    $("#createTransactionBtn").click(() => {
        $("#createTransactionModal").modal("show");
        // Lấy ngày và giờ hiện tại
        var now = new Date();

        // Format ngày và giờ hiện tại thành chuỗi "YYYY-MM-DDTHH:mm"
        var formattedDateTime = now.toISOString().slice(0, 16);

        // Đặt giá trị cho trường nhập liệu datetime-local
        $('#dateInput').val(formattedDateTime);
        $('#createTransaction').click(function () {
            
            var title = $('#titleInput').val();
            var amount = $('#amountInput').val();
            var description = $('#descriptionInput').val();
            var categoryId = $('#categorySelect').val();
            var type = categoryId != "00000000-0000-0000-0000-000000000000" ? 1 : parseInt($('#typeInput').val());
            var dateTime = $('#dateInput').val();

            $.ajax({
                url: "/transaction/create",
                type: "POST",
                headers: {
                    "Authorization": "Bearer "+ localStorage.getItem("jwtToken")
                },
                contentType: "application/json",
                data: JSON.stringify({
                    title: title,
                    amount: amount,
                    categoryId: categoryId,
                    description: description,
                    type: type,
                    date: dateTime
                }),
                success: function(response) {
                    console.log("Create successful:", response);
                    toastr.options = {"positionClass": "toast-bottom-right", "timeOut": 3000};                   
                    toastr.success("Tạo transaction thành công!");
                    setTimeout(function() {
                        window.location.href = "/home";
                    }, 1000);
                },
                error: function(xhr, status, error) {
                    // Xử lý lỗi nếu có
                    console.error("Tạo transaction lỗi:", xhr.responseJSON.message);
                    toastr.options = {"positionClass": "toast-bottom-right", "timeOut": 3000};
                    toastr.error(xhr.responseJSON.message);
                }
            })
            // Xử lý dữ liệu theo yêu cầu của bạn
        });


    })
});