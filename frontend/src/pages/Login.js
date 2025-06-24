import React from 'react';

function Login() {
    return (
        <div className="Login">
            <h2>Login</h2>
            <form id="loginForm">
                <input type="text" id="username" placeholder="Username" required />
                <input type="password" id="password" placeholder="Password" required />
                <button type="submit">Login</button>
                <div id="errorMsg" class="error"></div>
            </form>
        </div>

    );
}
export default Login;