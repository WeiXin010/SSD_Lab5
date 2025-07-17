const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');
const fs = require('fs');
const os = require('os');
const path = require('path');
const { error } = require('console');

async function testHomePage() {
    const options = new chrome.Options().addArguments('--ignore-certificate-errors');
    let driver = await new Builder().forBrowser('chrome').usingServer('http://localhost:4444/wd/hub').setChromeOptions(options).build();
    try {
        // Navigate to your React app URL
        await driver.get('https://chigga123.duckdns.org/login');

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

        // Clear and enter test data
        await emailField.clear();
        await emailField.sendKeys('test@example.com');

        await passwordField.clear();
        await passwordField.sendKeys('asdasd');
        await loginButton.click();

        // Wait for error message to appear
        const errorMsg = await driver.wait(until.elementLocated(By.id('errorMsg')), 5000);
        await driver.wait(
            until.elementIsVisible(errorMsg),
            5000
        );

        const errorText = await errorMsg.getText();

        if (!errorText.toLowerCase().includes('invalid')) {
            throw new Error('Expected error message did not appear');
        }

        console.log('✅ Login behavior test passed for invalid credentials');


    } catch (err) {
        console.log('Test failed: ', err);
    } finally {
        await driver.quit();
    }
}

testHomePage();