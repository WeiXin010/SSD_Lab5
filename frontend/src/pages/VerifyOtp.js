import React, { useState } from 'react';

function VerifyOtp() {
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleOTP = async (e) => {
        e.preventDefault();

        const email = localStorage.getItem('pendingEmail');
        if (!email) {
            setStatus("No email found. Please login again.");
            return;
        }

        try {
            const response = await fetch('/api/otp', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: email, otpCode: otp }),
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
        <div>
            <h2>Enter OTP</h2>
            <form onSubmit={handleVerify}>
                <input
                    type="text"
                    value={otp}
                    onChange={(e) => setOtp(e.target.value)}
                    placeholder="Enter OTP"
                    required
                />
                <button type="submit">Verify</button>
            </form>
            {status && <p>{status}</p>}
        </div>
    );
}

export default VerifyOtp;