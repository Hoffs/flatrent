import React, { ReactNode } from "react";
import ContentLoader from "react-content-loader";

const TextRowLoader = ({
    className,
    rows,
    width,
    indent = false,
}: {
    className?: string;
    rows: number;
    width: number;
    indent?: boolean;
}) => (
    <ContentLoader
        style={{ width: "100%", height: (rows * 20).toString() }}
        className={className}
        height={rows * 20}
        width={width}
        speed={2}
        primaryColor="#f3f3f3"
        secondaryColor="#ecebeb"
        baseUrl={window.location.pathname}
    >
        {generateRows(rows, width, indent)}
    </ContentLoader>
);

const generateRows = (rows: number, width: number, indent: boolean): ReactNode[] => {
    return Array(rows)
        .fill("")
        .map((_, idx) => {
            const randDiff = Math.random() * (width / 4);
            return (
                <rect
                    key={idx}
                    x={idx === 0 && indent ? "20" : "0"}
                    y={(idx * 20 + 5).toString()}
                    rx="3"
                    ry="3"
                    width={idx === 0 && indent ? (width - 20 - randDiff).toString() : (width - randDiff).toString()}
                    height="10"
                />
            );
        });
};

export default TextRowLoader;
