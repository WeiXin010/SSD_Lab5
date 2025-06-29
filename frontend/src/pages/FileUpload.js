import React, { useState } from 'react';
import './App.css';

function FileUpload() {
  const [file, setFile] = useState(null);
  const [status, setStatus] = useState('');

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
    setStatus('');
  };

  const handleUpload = async () => {
    if (!file) {
      setStatus('No file selected');
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
      // Replace with your actual backend endpoint
      const response = await fetch('/api/fileUpload', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        setStatus('File uploaded successfully!');
      } else {
        setStatus('Upload failed.');
      }
    } catch (error) {
      console.error('Error:', error);
      setStatus('Upload failed (network error).');
    }
  };

  return (
    <div className="App">
      <h2>React File Upload</h2>
      <input type="file" onChange={handleFileChange} />
      <button onClick={handleUpload}>Upload</button>
      {status && <p>{status}</p>}
    </div>
  );
}

export default App;
