import React, { useState, useEffect } from "react";
import axios from "axios";

const DiscountComponent = () => {
    const [discount, setDiscount] = useState("");
    const [messageId, setMessageId] = useState("");
    const [popReceipt, setPopReceipt] = useState("");
    const [showApply, setShowApply] = useState(false);
    const [showReturn, setShowReturn] = useState(false);

    useEffect(() => {
        axios.get("https://localhost:7092/api/Discount").then(res => {
            if (res.data.discount) {
                setDiscount(res.data.discount);
                setMessageId(res.data.messageId);
                setPopReceipt(res.data.popReceipt);
                setShowApply(true);
            }
        });
    }, []);

    const applyDiscount = () => {
        setShowApply(false);
        setShowReturn(true);
    };

    const returnDiscount = () => {
        axios.post(`https://localhost:7092/api/Discount/return?messageId=${messageId}&popReceipt=${popReceipt}`)
            .then(() => {
                setDiscount("");
                setShowReturn(false);
            });
    };

    return (
        <div>
            <h1>Discount App</h1>
            {discount && <h2>Endirim kodu: {discount}</h2>}
            {showApply && <button onClick={applyDiscount}>Endirim T?tbiq Et</button>}
            {showReturn && <button onClick={returnDiscount}>Endirimi ��xart</button>}
        </div>
    );
};

export default DiscountComponent;
