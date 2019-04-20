export const getImageUrl = (id: string): string => {
    return `/api/image/${id}`;
};

export const avatarUrl = (id: string): string => {
    return `/api/user/${id}/avatar`;
};
