# Automated API Testing Documentation

## Introduction

This document provides an overview of the automated testing approach for the CocktailDB API. The testing includes functional and non-functional tests to ensure the API's reliability, functionality, performance, and security.

## Functional Tests

### Test Case 1: Search Ingredients By Name

**URL:** `/api/json/v1/1/search.php?i=vodka`

**Expected Behavior:** The API should return details of the ingredient "Vodka".

**Assertions:**
- Ingredient ID should be "1".
- Ingredient should be "Vodka".
- Type should be "Vodka".
- Alcohol should be "Yes".
- ABV should be "40".

### Test Case 2: Search Cocktails By Name

**URL:** `/api/json/v1/1/search.php?s=margarita`

**Expected Behavior:** The API should return details of the cocktail "Margarita".

**Assertions:**
- Drink name should be "Margarita".
- Category should be "Ordinary Drink".
- Alcoholic should be "Alcoholic".
- Glass should be "Cocktail glass".
- Instructions should contain proper content.

### Test Case 3: Search Non-Alcoholic Ingredients By Name

**Custom Test**

**Expected Behavior:** The API should return details of the non-alcoholic ingredient "Orange juice".

**Assertions:**
- Ingredient name should be "Orange juice".
- Type should be "Non-alcoholic".
- Alcohol should be null.
- ABV should be null.

### Test Case 6: Search Cocktails By Name (Case Insensitive)

**Objective:** To verify that the API can search for cocktails by name in a case-insensitive manner.

**URL:** `/api/json/v1/1/search.php?s=MaRgArItA`

**Expected Behavior:** The API should return details of the cocktail "Margarita" regardless of the case used in the search query.

**Expected Response:** Status Code 200 OK

**Assertions:**
- The name of the returned cocktail should be "Margarita" (case-insensitive comparison).

## Non-Functional Tests

### Non-Functional Test 1: Performance Test

**Objective:** To assess the system's performance by simulating multiple users accessing the API simultaneously.

**Framework:** Artillery

### Non-Functional Test 2: Security Scanning Test

**Objective:** To identify security vulnerabilities by running a security scan on the API.

**Framework:** OWASP ZAP

---

## Frameworks Used for Automated Testing

1. **Artillery Framework for Performance Testing:**
   - Artillery is an open-source load testing framework for assessing performance.
   - It supports creating load testing scenarios and monitoring metrics.

2. **OWASP ZAP for Security Scanning:**
   - OWASP ZAP is a widely-used security testing tool for identifying vulnerabilities.
   - It offers scanning options and generates security reports.

---

## Conclusion

Automated testing ensures the CocktailDB API's functionality, performance, and security. Functional and non-functional tests are crucial to delivering a reliable and robust API.
