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
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                let message = 'Login Failed';

                try {
                    const text = await response.text(); // ✅ safer
                    const data = text ? JSON.parse(text) : null;
                    if (data?.message) message = data.message;
                } catch (e) {
                    console.warn('Failed to parse JSON:', e);
                }

                setError(message);
            } else {
                setError('');
                alert('Login Successful!');
            }
        } catch (err) {
            console.error('Fetch error:', err);
            setError(err.message || 'Network Error');
        }
    };

    return (
        <div className="Login">
            <h2>Login</h2>
            <form id="loginForm" onSubmit={handleSubmit}>
                <input type="email" name="email" id="email" placeholder="Email" required />
                <input type="password" name="password" id="password" placeholder="Password" required />
                <button type="submit">Login</button>
                {error && <div id="errorMsg" className="error">{error}</div>}
            </form>
        </div>
    );
}

export default Login;