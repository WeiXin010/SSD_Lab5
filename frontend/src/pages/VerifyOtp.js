import React, { useState } from 'react';

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

        const currentTime = new Date().toISOString();

        try {
            const response = await fetch('/api/verifyotp', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ 
                    email: email, 
                    otpCode: otp,
                    submittedTime: currentTime
                }),
            });

            if (response.ok) {
                const data = await response.json();
                console.log(data.message); // Or use it to update state, etc.
                setStatus("OTP verified! Logging in...");
                alert('Login Successful!');
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
