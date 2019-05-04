import React from "react";
import Styles from "./Footer.module.css";

const Footer = () => {
    return (
        <footer className={Styles.footer}>
            <span className={Styles.footerText}>© {new Date().getUTCFullYear()} Cosy, Inc.</span>
        </footer>
    );
};

export default Footer;
