import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function VerifyOtp() {
    const [otp, setOtp] = useState('');
    const [status, setStatus] = useState('');

    const handleVerify = async (e) => {
        e.preventDefault();
        const email = localStorage.getItem('pendingEmail'); // grab from login step

        if (!email) {
            setStatus("No email found. Please login again.");
            return;
        }

        try {
            const response = await fetch('/api/login/verify-otp', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, otpCode: otp }),
            });

            if (response.ok) {
                const data = await response.json();
                setStatus("OTP verified! Logging in...");
                // Optionally: navigate to dashboard/home
            } else {
                const error = await response.json();
                setStatus(error.message || "OTP verification failed.");
            }
        } catch (err) {
            setStatus("An error occurred while verifying OTP.");
            console.error(err);
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
