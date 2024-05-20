$.ajax({
    url: '/accountprofile/getaccountprofile',
    method: 'GET',
    headers:{
        "Authorization": "Bearer "+ localStorage.getItem("jwtToken")
    },
    success:(response)=>{
             console.log(response)
    }
});