$(document).ready(function () {
    const formatDate = (isoString) => {
        const date = new Date(isoString);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-based in JavaScript
        const year = date.getFullYear();
        return `${month}/${day}/${year}`;
    };
    $.ajax({
        url: '/accountprofile/getaccountprofile',
        method: 'GET',
        headers:{
            "Authorization": "Bearer "+ localStorage.getItem("jwtToken")
        },
        success:(response)=>{
            $("#firstNameInput").val(response.firstName)
            $("#lastNameInput").val(response.lastName)
            $("#emailInput").val(response.email)
            $("#addressInput").val(response.address)
            $("#currencyInput").val(response.currency)
            $("#name").text(response.firstName+' '+response.lastName)
            $("#dateOfBirth").text(formatDate(response.dateOfBirth))
            $("#address").text(response.address)
            console.log(response)
        }
    });
    
    $("#updateProfile").on("click", () => {

        $.ajax({
            url: '/accountprofile/update',
            method: 'POST',
            headers:{
                "Authorization": "Bearer "+ localStorage.getItem("jwtToken")
            },
            success:(response)=>{
                $("#firstNameInput").val(response.firstName)
                $("#lastNameInput").val(response.lastName)
                $("#emailInput").val(response.email)
                $("#addressInput").val(response.address)
                $("#currencyInput").val(response.currency)
                $("#name").text(response.firstName+' '+response.lastName)
                $("#dateOfBirth").text(formatDate(response.dateOfBirth))
                $("#address").text(response.address)
                console.log(response)
            }
        });
        
    })
    
})

