import React, { ChangeEvent } from "react";
import Card from "../../components/Card";
import Checkbox from "../../components/Checkbox";
import UserService, { Policies } from "../../services/UserService";
import Styles from "./FlatFilters.module.css";

interface IFlatFilterProps {
    onPageCountChange: (count: number) => void;
    defaultRented?: boolean;
    onShowRentedChange: (showRented: boolean) => void;
}

const FlatFilters = ({ onPageCountChange, defaultRented = false, onShowRentedChange }: IFlatFilterProps) => {
    const handleCountChange = (event: ChangeEvent<HTMLSelectElement>) => onPageCountChange(Number(event.target.value));
    const handleRentedChange = (state: boolean) => onShowRentedChange(state);

    return (
        <Card className={Styles.card}>
            {/*
      <select onChange={handleCountChange}>
        <option value={20}>20</option>
        <option value={40}>40</option>
        <option value={80}>80</option>
      </select>
      */}
            <Checkbox checked={defaultRented} onChange={handleRentedChange} text="Rodyti išnuomotus" />
        </Card>
    );
};

export default FlatFilters;
