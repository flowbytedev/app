





window.monacoInterop = {
    init: function (editorId, language, dotNetHelper) {
        require.config({ paths: { 'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs' } });
        require(['vs/editor/editor.main'], function () {
            window.editorInstance = monaco.editor.create(document.getElementById(editorId), {
                value: "-- Write your SQL here",
                language: language,
                theme: "vs-light",
                automaticLayout: true,
                minimap: { enabled: false }
            });

            window.editorInstance.onDidChangeModelContent(() => {
                const value = window.editorInstance.getValue();
                dotNetHelper.invokeMethodAsync('OnEditorContentChanged', value);
            });
        });
    },
    getValue: function () {
        return window.editorInstance.getValue();
    },
    setValue: function (value) {
        window.editorInstance.setValue(value);
    }
};
