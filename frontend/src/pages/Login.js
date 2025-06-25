import React, { useState } from 'react';


function Login() {
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const email = e.target.email.value;
        const password = e.target.password.value;

        try {
            const response = await fetch('/api/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                // Extract error message from response
                const data = await response.json();
                setError(data.message || 'Login Failed');
            }
            else {
                setError('');
                alert('Login Successful!');
            }
        }
        catch (err) {
            setError('Network Error');
        }
    };

        return (
            <div className="Login">
                <h2>Login</h2>
                <form id="loginForm" onSubmit={handleSubmit}>
                    <input type="email" id="email" placeholder="Email" required />
                    <input type="password" id="password" placeholder="Password" required />
                    <button type="submit">Login</button>
                    {error && <div id="errorMsg" className="error">{error}</div>}
                </form>
            </div>

        );
    }

    export default Login;