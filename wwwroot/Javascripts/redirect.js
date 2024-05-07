if (!localStorage.getItem('jwtToken')) {
    localStorage.setItem('redirectToLogin', true);
    window.location.href = "/login";
}
