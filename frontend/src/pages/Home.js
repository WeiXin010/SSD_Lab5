import cat from './cat.png';
import '../App.css';
import React from 'react';

function Home() {
    return (
        <div className="App">
            <header className="App-header">
                <img src={cat} className="App-logo" alt="logo" />
                <h1 className="test-title">DU DU DU MAX VERSTAPPEN!!</h1>
                <p>
                    Tell me why! Aint nothing but a heart break.
                </p>
                <p>
                    Ruby-chan! HAIIIII! Nani gatsuki?
                </p>
                <p>
                    Choco Minto youri mo.....
                </p>
                <a
                    className="App-link"
                    href="https://reactjs.org"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Learn React
                </a>
            </header>
        </div>
    );
}

export default Home;