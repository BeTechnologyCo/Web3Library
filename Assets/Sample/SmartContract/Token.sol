// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "@openzeppelin/contracts/token/ERC20/extensions/ERC20Burnable.sol";

// This contract was generate with https://docs.openzeppelin.com/contracts/4.x/wizard
// It permit to mint token publicly, depose and withdraw matic
// Address on mumbai 0x685576c3a592088eA9CA528b342D05087a64b6E7
// https://mumbai.polygonscan.com/token/0x685576c3a592088eA9CA528b342D05087a64b6E7
contract Web3Token is ERC20, ERC20Burnable {
    constructor() ERC20("Web3Token", "W3T") {}

    event  Deposit(address indexed user, uint256 amount);
    event  Withdrawal(address indexed user, uint256 amount);

    mapping(address=>uint256) public amountDeposed;

    function mint(address to, uint256 amount) public  {
        _mint(to, amount);
    }

    function deposit() public payable {
        amountDeposed[msg.sender] += msg.value;
        emit Deposit(msg.sender, msg.value);
    }
    function withdraw() public {
        require(amountDeposed[msg.sender] > 0, "No amount to withdraw");
        uint256 balance = amountDeposed[msg.sender];
        amountDeposed[msg.sender] = 0;
        (bool sent, ) = msg.sender.call{value: balance}("");
        require(sent, "Failed to send Ether");
        emit Withdrawal(msg.sender, balance);
    }
}
