mergeInto(LibraryManager.library, {
    WebGLStorage_Save: function (key, data) {
        var keyStr = UTF8ToString(key);
        var dataStr = UTF8ToString(data);
        localStorage.setItem(keyStr, dataStr);
    },
    WebGLStorage_Load: function (key) {
        var keyStr = UTF8ToString(key);
        var data = localStorage.getItem(keyStr);
        return data ? Pointer_stringify(data) : null;
    },
    WebGLStorage_Delete: function (key) {
        var keyStr = UTF8ToString(key);
        localStorage.removeItem(keyStr);
    }
});
