const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');
const fs = require('fs');
const os = require('os');
const path = require('path');

async function testHomePage() {
    const options = new chrome.Options().addArguments('--ignore-certificate-errors');
    let driver = await new Builder().forBrowser('chrome').usingServer('http://localhost:4444/wd/hub').setChromeOptions(options).build();
    try {
        // Navigate to your React app URL
        await driver.get('https://chigga123.duckdns.org');

        // Wait for the header element with class "test-title" and check its text
        let header = await driver.wait(
            until.elementLocated(By.className('test-title')),
            5000
        );
        let headerText = await header.getText();
        console.log('Header Text: ', headerText);
        if (headerText !== 'DU DU DU MAX VERSTAPPEN!!') {
            throw new Error('Header text does not match');
        }
        console.log('Arrived on Home page');

        // Find the Login link by its href attribute (React Router uses actual href)
        let loginLink = await driver.findElement(By.xpath("//a[@href='/login' and text()='Login']"));

        // Click the Login Link
        await loginLink.click();

        // Wait for URL to change to /login
        await driver.wait(until.urlContains('/login'), 5000);

        // Wait for email field to be ready.
        const emailField = await driver.findElement(By.id('email'), 5000);
        const emailVisible = await emailField.isDisplayed();

        // Wait for password field to be ready.
        const passwordField = await driver.findElement(By.id('password'), 5000);
        const passwordVisible = await passwordField.isDisplayed();

        // Wait for Login button to be ready
        const loginButton = await driver.wait(until.elementLocated(By.css('button[type="submit"]')), 5000);
        const loginVisible = await loginButton.isDisplayed();
        const loginEnabled = await loginButton.isEnabled();

        if (!emailVisible || !passwordVisible) {
            throw new Error('Email or Password input not visible');
        }
        if (!loginVisible) {
            throw new Error('Login button not visible');
        }
        if (!loginEnabled) {
            throw new Error('Login button is disabled');
        }

        console.log('✅ Email, Password inputs are visible and Login button is clickable');


    } catch (err) {
        console.log('Test failed: ', err);
    } finally {
        await driver.quit();
    }
}

testHomePage();